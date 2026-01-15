using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;

namespace DocumentFlowing.Models.Authorization;

public class LoginModel
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ITokenService _tokenService;
    
    public LoginModel(IAuthorizationService authorizationService, ITokenService tokenService)
    {
        _authorizationService = authorizationService;
        _tokenService = tokenService;
    }
    
    public async Task<int?> LoginAsync(string email, string password)
    {
        var roleId = await  _authorizationService.LoginAsync(email, password);
        
        return roleId;
    }
    
    public async Task<int?> LoginByRefreshTokenAsync()
    {
        return await _authorizationService.TryAutoLoginAsync()? _tokenService.GetUserInfo().RoleId : null;
    }

    public async Task CheckRefreshTokenExipredAsync()
    {
        if (_tokenService.IsRefreshTokenExpires())
        {
            await _tokenService.GetNewRefreshTokenAsync();
        }
    }
}