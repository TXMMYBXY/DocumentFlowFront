using DocumentFlowing.Client.Authorization.ViewModel;
using DocumentFlowing.Models;

namespace DocumentFlowing.Interfaces.Services;

public interface ITokenService
{
    void SaveTokens(LoginResponse loginResponse);
    void SaveRefreshToken(RefreshTokenResponseViewModel refreshTokenResponse);
    string GetAccessToken();
    string GetRefreshToken();
    UserInfoDto GetUserInfo();
    int? GetRefreshTokenId();
    int? GetUserId();
    bool HasValidToken();
    bool IsRefreshTokenValid();
}