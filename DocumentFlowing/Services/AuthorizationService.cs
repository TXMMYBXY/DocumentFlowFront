using AutoMapper;
using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Client.Authorization.ViewModels;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using System.Windows;

namespace DocumentFlowing.Services;

public class AuthorizationService :  IAuthorizationService
{
    private readonly ITokenService _tokenService;
    private readonly IAuthorizationClient _authorizationClient;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;

    public AuthorizationService(
        ITokenService tokenService, 
        IAuthorizationClient authorizationClient,
        IMapper mapper,
        INavigationService navigationService)
    {
        _tokenService = tokenService;
        _authorizationClient = authorizationClient;
        _mapper = mapper;
        _navigationService = navigationService;
    }
    public async Task<bool> TryAutoLoginAsync()
    {
        try
        {
            var request = new RefreshTokenToLoginRequestViewModel
            {
                RefreshToken = _tokenService.GetRefreshToken()
            };
            
            if (!string.IsNullOrEmpty(request.RefreshToken))
            {
                // Запрос к API за доступом
                
                var refreshToken 
                    = await _authorizationClient.RequestForAccessAsync<RefreshTokenToLoginRequestViewModel, RefreshTokenToLoginResponseViewModel>
                        (request, "authorization/request-for-access");

                if (refreshToken.IsAllowed)
                {
                    if (refreshToken.RefreshTokenDto != null)
                    {
                        // Сохраняем новые токены
                        var refreshTokenResponseViewModel = _mapper.Map<RefreshTokenResponseViewModel>(refreshToken);
                        _tokenService.SaveRefreshToken(refreshTokenResponseViewModel);
                    }

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

            var response = await _authorizationClient.LoginAsync<LoginRequestDto, LoginResponseDto>(
                loginRequest, "authorization/login");

            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                // Сохраняем токены
                _tokenService.SaveTokens(response);
                // LoginSuccessful?.Invoke(this, EventArgs.Empty);

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
