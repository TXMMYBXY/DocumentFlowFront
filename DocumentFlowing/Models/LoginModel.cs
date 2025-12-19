using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using System.Windows;

namespace DocumentFlowing.Models;

public class LoginModel
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ITokenService _tokenService;
    
    public LoginModel(IAuthorizationService authorizationService, ITokenService tokenService)
    {
        _authorizationService = authorizationService;
        _tokenService = tokenService;
    }
    
    public async Task LoginAsync(string email, string password)
    {
        await  _authorizationService.LoginAsync(email, password);
    }
    
    public async Task<int?> LoginByRefreshTokenAsync()
    {
        return await _authorizationService.TryAutoLoginAsync()? _tokenService.GetUserInfo().RoleId : null;
    }
}