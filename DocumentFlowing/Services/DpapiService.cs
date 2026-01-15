using DocumentFlowing.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace DocumentFlowing.Services;

public class DpapiService : IDpapiService
{
    private readonly byte[] _entropy;

    public DpapiService()
    {
        _entropy = _GetEntropy();
    }
    
    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return string.Empty;

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = ProtectedData.Protect(plainBytes, _entropy, DataProtectionScope.CurrentUser);
        
        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText)) return string.Empty;

        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        byte[] plainBytes = ProtectedData.Unprotect(encryptedBytes, _entropy, DataProtectionScope.CurrentUser);
        
        return Encoding.UTF8.GetString(plainBytes);
    }
    
    private static byte[] _GetEntropy()
    {
        string appData = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData);
        
        string uniqueKey = $"{appData}|{Environment.MachineName}|DocumentFlow";
        
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(uniqueKey));
        }
    }
}