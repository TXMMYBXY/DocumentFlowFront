using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using System.Windows;

namespace DocumentFlowing.Models;

public class LoginModel
{
    private readonly IAuthorizationClient _authorizationClient;
    private readonly ITokenService _tokenService;
    private readonly INavigationService _navigationService;
    
    public LoginModel(
        IAuthorizationClient authorizationClient, 
        ITokenService tokenService,
        INavigationService navigationService)
    {
        _authorizationClient = authorizationClient;
        _tokenService = tokenService;
        _navigationService = navigationService;
    }
    
    public async void LoginAsync(string email, string password)
    {
        // Блокируем кнопку во время запроса
        // LoginButton.IsEnabled = false;
        // LoginButton.Content = "Вход...";

        try
        {
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await _authorizationClient.LoginAsync<LoginRequest, LoginResponse>(
                loginRequest, "authorization/login");

            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                // Сохраняем токены
                _tokenService.SaveTokens(response);
                // LoginSuccessful?.Invoke(this, EventArgs.Empty);

                // Открываем соответствующее окно на основе RoleId из ответа
                if (response.UserInfo != null)
                {
                    _navigationService.NavigateToRole(response.UserInfo.RoleId);
                    Application.Current.MainWindow?.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка получения информации о пользователе.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Неверный email или пароль.", "Ошибка входа",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            // LoginButton.IsEnabled = true;
            // LoginButton.Content = "Login";
        }
    }
    
    private void _CheckSavedToken()
    {
        if (_tokenService.HasValidToken())
        {
            // Если есть валидный токен, сразу открываем нужное окно
            var userInfo = _tokenService.GetUserInfo();
            if (userInfo != null)
            {
                _navigationService.NavigateToRole(userInfo.RoleId);
                Application.Current.MainWindow?.Close();
            }
        }
    }
    
}