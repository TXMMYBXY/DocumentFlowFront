using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using System.Windows;

namespace DocumentFlowing.Services;

public class AuthorizationService :  IAuthorizationService
{
    private readonly ITokenService _tokenService;
    private readonly IAuthorizationClient _authorizationClient;

    public AuthorizationService(
        ITokenService tokenService, 
        IAuthorizationClient authorizationClient)
    {
        _tokenService = tokenService;
        _authorizationClient = authorizationClient;
    }
    
    public async Task<bool> TryAutoLoginAsync()
    {
        try
        {
            var request = new RefreshTokenToLoginRequestDto
            {
                RefreshToken = _tokenService.ReturnRefreshToken()
            };
            
            if (!string.IsNullOrEmpty(request.RefreshToken))
            {
                var refreshToken 
                    = await _authorizationClient.RequestForAccessAsync(request, "authorization/request-for-access");

                if (refreshToken.IsAllowed)
                {
                    return true;
                }
            }
            
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Auto-login failed: {ex.Message}");
            return false;
        }
    }

    public async Task<int?> LoginAsync(string email, string password)
    {
        try
        {
            var loginRequest = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            var response = await _authorizationClient.LoginAsync(loginRequest, "authorization/login");

            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                _tokenService.SaveTokens(response);

                return _tokenService.GetUserInfo().RoleId;
            }

            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
            
            return null;
        }
    }
}
