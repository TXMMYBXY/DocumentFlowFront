using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models.Authorization;
using DocumentFlowing.ViewModels.Base;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Authorization;

public class LoginViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly INavigationService _navigationService;
    private readonly LoginModel _loginModel;
    
    private string _email;
    private string _password;
    private bool _isLoading = false;
    
    public string Email
    {
        get => _email;
        set => SetField(ref _email, value);
    }
    
    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            SetField(ref _isLoading, value);
            OnPropertyChanged(nameof(LoginButtonText));
        }
    }
    
    public string LoginButtonText => IsLoading ? string.Empty : "Войти";
    public Task Initialization { get; private set; }
    public ICommand LoginCommand { get; }

    public LoginViewModel(
        IAuthorizationService authorizationService, 
        ITokenService tokenService,
        INavigationService navigationService)
    {
        _navigationService = navigationService;    
        _loginModel = new LoginModel(authorizationService, tokenService);
            
        Initialization = _InitializeAsync();
        LoginCommand = new RelayCommand(
            async () =>
            {
                IsLoading = true;
                _Login();
            },
            () =>
                !string.IsNullOrWhiteSpace(Email)
                  || !string.IsNullOrWhiteSpace(Password));
    }

    private async Task _InitializeAsync()
    {
        try
        {
            await _loginModel.CheckRefreshTokenExipredAsync();
            var roleId = await _loginModel.LoginByRefreshTokenAsync();
            if (roleId.HasValue)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _navigationService.NavigateToRole(roleId.Value);
                });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Auto-login failed: {ex.Message}");
        }
    }

    private async Task _Login()
    {
        var roleId = await _loginModel.LoginAsync(Email, Password);

        if (roleId.HasValue)
        {
            _navigationService.NavigateToRole(roleId);
        }

        IsLoading = false;
    }
}
