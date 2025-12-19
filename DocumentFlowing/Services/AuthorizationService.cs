using AutoMapper;
using DocumentFlowing.Client.Authorization.ViewModels;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;

namespace DocumentFlowing.Services;

public class AuthorizationService :  IAuthorizationService
{
    private readonly ITokenService _tokenService;
    private readonly IAuthorizationClient _authorizationClient;
    private readonly IMapper _mapper;

    public AuthorizationService(
        ITokenService tokenService, 
        IAuthorizationClient authorizationClient,
        IMapper mapper)
    {
        _tokenService = tokenService;
        _authorizationClient = authorizationClient;
        _mapper = mapper;
    }
    public async Task<bool> TryAutoLoginAsync()
    {
        try
        {
            var refreshTokenViewModel = new RefreshTokenViewModel
            {
                Token = _tokenService.GetRefreshToken()
            };
            
            
            if (!string.IsNullOrEmpty(refreshTokenViewModel.Token) && _tokenService.IsRefreshTokenValid())
            {
                // Запрос к API за доступом
                var request = new RefreshTokenToLoginRequestViewModel { RefreshToken = refreshTokenViewModel.Token };
                var refreshToken 
                    = await _authorizationClient.RequestForAccessAsync<RefreshTokenToLoginRequestViewModel, RefreshTokenToLoginResponseViewModel>
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
