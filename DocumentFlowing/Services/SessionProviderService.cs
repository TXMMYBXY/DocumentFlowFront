using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Authorization;

namespace DocumentFlowing.Services;

public class SessionProviderService : ISessionProviderService
{
    private readonly ITokenService _tokenService;
    private readonly INavigationService _navigationService;

    public bool IsAuthenticated => _tokenService.IsRefreshTokenValid();

    public int GetUserRoleId()
    {
        return  _tokenService.GetUserInfo().RoleId;
    }

    public event EventHandler? LogoutRequested;
    
    public SessionProviderService(ITokenService tokenService, INavigationService navigationService)
    {
        _tokenService = tokenService;
        _navigationService = navigationService;
    }
    
    public async Task LogoutAsync()
    {
        _tokenService.ClearTokens();
        _navigationService.NavigateTo<LoginView>();
        
        await Task.CompletedTask;
    }
}