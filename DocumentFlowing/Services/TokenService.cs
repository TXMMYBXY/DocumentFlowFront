using AutoMapper;
using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Client.Authorization.ViewModels;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using Microsoft.Win32;

namespace DocumentFlowing.Services;

public class TokenService : ITokenService
{
    private const string RegistryPath = @"Software\DocumentFlowing\Tokens";
    private readonly IDpapiService _dpapiService;
    private readonly IAuthorizationClient _authorizationClient;
    private readonly IMapper _mapper;

    public TokenService(
        IDpapiService dpapiService, 
        IAuthorizationClient authorizationClient,
        IMapper mapper)
    {
        _dpapiService = dpapiService;
        _authorizationClient = authorizationClient;
        _mapper = mapper;
    }
    
    public void SaveTokens(LoginResponseDto loginResponseDto)
    {
        try
        {
            if (loginResponseDto == null) return;

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath))
            {
                if (key == null) return;

                // Сохраняем access token
                if (!string.IsNullOrEmpty(loginResponseDto.AccessToken))
                {
                    var encryptedAccessToken = _dpapiService.Encrypt(loginResponseDto.AccessToken);
                    key.SetValue("AccessToken", encryptedAccessToken);
                    key.SetValue("AccessTokenExpires", loginResponseDto.ExpiresAt);
                }

                // Сохраняем refresh token
                if (loginResponseDto.RefreshTokenDto != null && !string.IsNullOrEmpty(loginResponseDto.RefreshTokenDto.Token))
                {
                    var encryptedRefreshToken = _dpapiService.Encrypt(loginResponseDto.RefreshTokenDto.Token);
                    key.SetValue("RefreshToken", encryptedRefreshToken);
                    key.SetValue("RefreshTokenExpires", loginResponseDto.RefreshTokenDto.ExpiresAt);
                }

                // Сохраняем информацию о пользователе
                if (loginResponseDto.UserInfo != null)
                {
                    key.SetValue("UserEmail", loginResponseDto.UserInfo.Email);
                    key.SetValue("UserFullName", loginResponseDto.UserInfo.FullName);
                    key.SetValue("RoleId", loginResponseDto.UserInfo.RoleId);
                    key.SetValue("DepartmentId", loginResponseDto.UserInfo.DepartmentId);

                    // Сохраняем userId из refresh token
                    if (loginResponseDto.RefreshTokenDto != null)
                    {
                        key.SetValue("UserId", loginResponseDto.RefreshTokenDto.UserId);
                    }
                }

                // Сохраняем тип токена
                key.SetValue("TokenType", loginResponseDto.TokenType);
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

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath))
            {
                if (key == null) return;
                
                if (refreshTokenResponse.Token != null && !string.IsNullOrEmpty(refreshTokenResponse.Token))
                {
                    var encryptedRefreshToken = _dpapiService.Encrypt(refreshTokenResponse.Token);
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

    public string ReturnAccessToken()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
            {
                if (key == null) return null;

                var encryptedToken = key.GetValue("AccessToken") as string;
                
                if (string.IsNullOrEmpty(encryptedToken)) return null;

                return _dpapiService.Decrypt(encryptedToken);
            }
        }
        catch
        {
            return null;
        }
    }

    public string ReturnRefreshToken()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
            {
                if (key == null) return null;

                var encryptedToken = key.GetValue("RefreshToken") as string;
                if (string.IsNullOrEmpty(encryptedToken)) return null;

                return _dpapiService.Decrypt(encryptedToken);
            }
        }
        catch
        {
            return null;
        }
    }

    public UserInfoDto GetUserInfo()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
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

    public int? GetUserId()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
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

    public async Task<string> GetNewAccessToken()
    {
        var request = new AccessTokenViewModelRequest
        {
            RefreshToken = ReturnRefreshToken(),
            UserId = GetUserId()
        };

        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            throw new NullReferenceException("Refresh token is out");
        }
        
        var token = await _authorizationClient
            .GetNewAccessTokenAsync<AccessTokenViewModelRequest, AccessTokenViewModelResponse>(request, "authorization/access");

        if (token == null)
        {
            throw new NullReferenceException("Refresh token is not got");
        }
        
        SaveTokens(_mapper.Map<LoginResponseDto>(token));

        return token.AccessToken;
    }

    public int? GetRefreshTokenId()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
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

    public void ClearTokens()
    {
        try
        {
            Registry.CurrentUser.DeleteSubKeyTree(RegistryPath, false);
        }
        catch
        {
            // Игнорируем ошибки, если ключ не существует
        }
    }

    public bool IsAccessTokenValid()
    {
        try
        {
            var token = ReturnAccessToken();
            if (string.IsNullOrEmpty(token)) return false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
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
                        return expiresAt > DateTime.Now;
                    }
                    else
                    {
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
    
    public bool IsRefreshTokenValid()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
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