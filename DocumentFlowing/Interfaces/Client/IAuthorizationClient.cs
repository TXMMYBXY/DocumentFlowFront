using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Client.Authorization.ViewModels;

namespace DocumentFlowing.Interfaces.Client;

public interface IAuthorizationClient
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string uri);
    Task<RefreshTokenToLoginResponseViewModel> RequestForAccessAsync(RefreshTokenToLoginRequestViewModel request, string uri);
    Task<AccessTokenViewModelResponse> GetNewAccessTokenAsync(AccessTokenViewModelRequest request, string uri);
    Task<RefreshTokenResponseViewModel> GetNewRefreshTokenAsync(RefreshTokenViewModelRequest request, string uri);
}