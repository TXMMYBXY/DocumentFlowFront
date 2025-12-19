using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using DocumentFlowing.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Authorization;

public class LoginViewModel : BaseViewModel
{
    private string _email;
    private string _password;
    private bool _isLoading = false;

    private readonly LoginModel _loginModel;

    public LoginViewModel(
        IAuthorizationClient authorizationClient, 
        ITokenService tokenService,
        INavigationService navigationService)
    {
        _loginModel = new LoginModel(authorizationClient, tokenService, navigationService);

        LoginCommand = new RelayCommand(
            () =>
            {
                IsLoading = true;
                _loginModel.LoginAsync(Email, Password);
            },
            () =>
                !string.IsNullOrWhiteSpace(Email)
                  || !string.IsNullOrWhiteSpace(Password));
    }
    
    public ICommand LoginCommand { get; }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }
    
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(LoginButtonText));
        }
    }
    
    public string LoginButtonText => IsLoading ? string.Empty : "Войти";
}
