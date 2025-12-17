namespace DocumentFlowing.Interfaces.Services;

public interface IDpapiService
{
    /// <summary>
    /// Шифрование с использованием DPAPI
    /// </summary>
    string Encrypt(string plainText);
    
    /// <summary>
    /// Дешифрование с использованием DPAPI
    /// </summary>
    string Decrypt(string encryptedText);
}