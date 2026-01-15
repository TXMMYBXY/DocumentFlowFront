namespace DocumentFlowing.Interfaces.Services;

/// <summary>
/// Класс шифрования рефреш-токенов с использованием DPAPI
/// </summary>
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