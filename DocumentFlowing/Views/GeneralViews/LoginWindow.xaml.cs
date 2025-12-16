using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Boss;
using DocumentFlowing.Views.User;
using DocumentFlowing.Views.Purchaser;
using System.Windows;

namespace DocumentFlowing.Views
{
    public partial class LoginWindow : Window
    {
        public event EventHandler LoginSuccessful;
        
        private readonly IAuthorizationClient _authorizationClient;
        private readonly ITokenService _tokenService;
        

        public LoginWindow(IAuthorizationClient authorizationClient, ITokenService tokenService)
        {
            _authorizationClient = authorizationClient;
            _tokenService = tokenService;
            InitializeComponent();

            // Проверяем, есть ли сохраненный токен
            _CheckSavedToken();
        }

        private void _CheckSavedToken()
        {
            if (_tokenService.HasValidToken())
            {
                // Если есть валидный токен, сразу открываем нужное окно
                var userInfo = _tokenService.GetUserInfo();
                if (userInfo != null)
                {
                    _OpenRoleSpecificWindow(userInfo.RoleId);
                    Close();
                }
            }
        }

        private async void _LoginClickAsync(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Пожалуйста, введите email и пароль.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Блокируем кнопку во время запроса
            LoginButton.IsEnabled = false;
            LoginButton.Content = "Вход...";

            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = LoginBox.Text,
                    Password = PasswordBox.Password
                };

                var response = await _authorizationClient.LoginAsync<LoginRequest, LoginResponse>(
                    loginRequest, "authorization/login");

                if (response != null && !string.IsNullOrEmpty(response.AccessToken))
                {
                    // Сохраняем токены
                    _tokenService.SaveTokens(response);
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);

                    // Открываем соответствующее окно на основе RoleId из ответа
                    if (response.UserInfo != null)
                    {
                        _OpenRoleSpecificWindow(response.UserInfo.RoleId);
                        Close();
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
                LoginButton.IsEnabled = true;
                LoginButton.Content = "Login";
            }
        }

        /// <summary>
        /// Метод для проверки роли пользователя 
        /// </summary>
        private void _OpenRoleSpecificWindow(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    MessageBox.Show("Admin");
                    new AdminMainView(_authorizationClient, _tokenService).Show();
                    break;
                case 2:
                    MessageBox.Show("Boss");
                    new BossMainView(_authorizationClient, _tokenService).Show();
                    break;
                case 3:
                    new PurchaserMainView().Show();
                    break;
                case 4:
                    new UserMainView().Show();
                    break;
            }
        }

        private void _ReturnBackClick(object sender, RoutedEventArgs e)
        {
            Close();

            Application.Current.MainWindow?.Show();
        }
    }
}