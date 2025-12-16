using DocumentFlowing.Client.Authorization.ViewModel;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace DocumentFlowing.Client.Services;

public class TokenService : ITokenService
{
    private static readonly byte[] _entropy = Encoding.UTF8.GetBytes("DocumentFlowEntropy");
    private const string _registryPath = @"Software\DocumentFlowing\Tokens";

    // Сохраняем токены в реестре Windows
    public void SaveTokens(LoginResponse loginResponse)
    {
        try
        {
            if (loginResponse == null) return;

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(_registryPath))
            {
                if (key == null) return;

                // Сохраняем access token
                if (!string.IsNullOrEmpty(loginResponse.AccessToken))
                {
                    var encryptedAccessToken = Encrypt(loginResponse.AccessToken);
                    key.SetValue("AccessToken", encryptedAccessToken);
                    key.SetValue("AccessTokenExpires", loginResponse.ExpiresAt);
                }

                // Сохраняем refresh token
                if (loginResponse.RefreshToken != null && !string.IsNullOrEmpty(loginResponse.RefreshToken.Token))
                {
                    var encryptedRefreshToken = Encrypt(loginResponse.RefreshToken.Token);
                    key.SetValue("RefreshToken", encryptedRefreshToken);
                    key.SetValue("RefreshTokenExpires", loginResponse.RefreshToken.ExpiresAt);
                }

                // Сохраняем информацию о пользователе
                if (loginResponse.UserInfo != null)
                {
                    key.SetValue("UserEmail", loginResponse.UserInfo.Email);
                    key.SetValue("UserFullName", loginResponse.UserInfo.FullName);
                    key.SetValue("RoleId", loginResponse.UserInfo.RoleId);
                    key.SetValue("DepartmentId", loginResponse.UserInfo.DepartmentId);

                    // Сохраняем userId из refresh token
                    if (loginResponse.RefreshToken != null)
                    {
                        key.SetValue("UserId", loginResponse.RefreshToken.UserId);
                    }
                }

                // Сохраняем тип токена
                key.SetValue("TokenType", loginResponse.TokenType);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving tokens: {ex.Message}");
        }
    }

    public void SaveRefreshToken(RefreshTokenResponseViewModel refreshTokenResponse)
    {
        try
        {
            if (refreshTokenResponse == null) return;

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(_registryPath))
            {
                if (key == null) return;
                
                if (refreshTokenResponse.Token != null && !string.IsNullOrEmpty(refreshTokenResponse.Token))
                {
                    var encryptedRefreshToken = Encrypt(refreshTokenResponse.Token);
                    key.SetValue("RefreshToken", encryptedRefreshToken);
                    key.SetValue("RefreshTokenExpires", refreshTokenResponse.ExpiresAt);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving tokens: {ex.Message}");
        }
    }

    // Получаем access token
    public string GetAccessToken()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
            {
                if (key == null) return null;

                var encryptedToken = key.GetValue("AccessToken") as string;
                if (string.IsNullOrEmpty(encryptedToken)) return null;

                return Decrypt(encryptedToken);
            }
        }
        catch
        {
            return null;
        }
    }

    // Получаем refresh token
    public string GetRefreshToken()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
            {
                if (key == null) return null;

                var encryptedToken = key.GetValue("RefreshToken") as string;
                if (string.IsNullOrEmpty(encryptedToken)) return null;

                return Decrypt(encryptedToken);
            }
        }
        catch
        {
            return null;
        }
    }

    // Получаем информацию о пользователе
    public UserInfoDto GetUserInfo()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
            {
                if (key == null) return null;

                var roleId = key.GetValue("RoleId") as int?;
                if (!roleId.HasValue) return null;

                return new UserInfoDto
                {
                    FullName = key.GetValue("UserFullName") as string,
                    Email = key.GetValue("UserEmail") as string,
                    RoleId = roleId.Value,
                    DepartmentId = key.GetValue("DepartmentId") as int? ?? 0
                };
            }
        }
        catch
        {
            return null;
        }
    }

    // Получаем userId
    public int? GetUserId()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
            {
                if (key == null) return null;

                return key.GetValue("UserId") as int?;
            }
        }
        catch
        {
            return null;
        }
    }

    // Получаем refresh token id
    public int? GetRefreshTokenId()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
            {
                if (key == null) return null;

                return key.GetValue("RefreshTokenId") as int?;
            }
        }
        catch
        {
            return null;
        }
    }

    // Очищаем токены (логаут)
    public static void ClearTokens()
    {
        try
        {
            Registry.CurrentUser.DeleteSubKeyTree(_registryPath, false);
        }
        catch
        {
            // Игнорируем ошибки, если ключ не существует
        }
    }

    // Проверяем, есть ли сохраненный токен
    public bool HasValidToken()
    {
        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token)) return false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
            {
                if (key == null) return false;

                // Проверяем срок действия токена
                var expiresAtStr = key.GetValue("AccessTokenExpires") as string;
                if (!string.IsNullOrEmpty(expiresAtStr))
                {
                    // Парсим дату в формате "dd.MM.yyyy HH:mm:ss"
                    if (DateTime.TryParseExact(expiresAtStr, "dd.MM.yyyy HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out DateTime expiresAt))
                    {
                        return expiresAt > DateTime.Now.AddMinutes(5); // Добавляем буфер в 5 минут
                    }
                    else
                    {
                        // Если не удалось распарсить, пробуем стандартный формат
                        if (DateTime.TryParse(expiresAtStr, out expiresAt))
                        {
                            return expiresAt > DateTime.Now.AddMinutes(5);
                        }
                    }
                }

                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    // Шифрование с использованием DPAPI
    private static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return string.Empty;

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = ProtectedData.Protect(plainBytes, _entropy, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encryptedBytes);
    }

    // Дешифрование с использованием DPAPI
    private static string Decrypt(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText)) return string.Empty;

        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        byte[] plainBytes = ProtectedData.Unprotect(encryptedBytes, _entropy, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(plainBytes);
    }

    // Проверяем срок действия refresh token
    public bool IsRefreshTokenValid()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath))
            {
                if (key == null) return false;

                var expiresAtStr = key.GetValue("RefreshTokenExpires") as string;
                if (!string.IsNullOrEmpty(expiresAtStr) &&
                    DateTime.TryParse(expiresAtStr, out DateTime expiresAt))
                {
                    return expiresAt > DateTime.UtcNow;
                }

                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}