using AutoMapper;
using DocumentFlowing.Client.Authorization.ViewModel;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;

namespace DocumentFlowing.Client.Services;

public class AuthorizationService :  IAuthorizationService
{
    private readonly ITokenService _tokenService;
    private readonly IGeneralClient _generalClient;
    private readonly IMapper _mapper;

    public AuthorizationService(
        ITokenService tokenService, 
        IGeneralClient generalClient,
        IMapper mapper)
    {
        _tokenService = tokenService;
        _generalClient = generalClient;
        _mapper = mapper;
    }
    public async Task<bool> TryAutoLoginAsync()
    {
        try
        {
            // // 1. Проверяем, есть ли валидный access token
            // if (_tokenService.HasValidToken())
            // {
            //     // Токен валиден - пользователь уже авторизован
            //     return true;
            // }
            //
            // 2. Если access token истек, но есть валидный refresh token
            
            var refreshTokenViewModel = new RefreshTokenViewModel
            {
                Token = _tokenService.GetRefreshToken()
            };
            
            
            if (!string.IsNullOrEmpty(refreshTokenViewModel.Token) && _tokenService.IsRefreshTokenValid())
            {
                // Запрос к API за доступом
                var request = new RefreshTokenToLoginRequestViewModel() { RefreshToken = refreshTokenViewModel.Token };
                var refreshToken 
                    = await _generalClient.PostResponseAsync<RefreshTokenToLoginRequestViewModel, RefreshTokenToLoginResponseViewModel>
                        (request, "authorization/request-for-access");

                if (refreshToken.IsAllowed)
                {
                    if (refreshToken.RefreshToken != null)
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
    
}
