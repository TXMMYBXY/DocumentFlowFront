using DocumentFlowing.Client;
using DocumentFlowing.Models;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Administration;
using DocumentFlowing.Views.Employee;
using DocumentFlowing.Views.Purchaser;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private string _selectedRole = string.Empty;
        private readonly GeneralClient _apiClient;
        public LoginView()
        {
            InitializeComponent();
            _apiClient = App.ServiceProvider.GetRequiredService<GeneralClient>();
        }

        private void Admin_Click(object sender, RoutedEventArgs e) => ShowLoginPanel("Administrator");
        private void AdminUser_Click(object sender, RoutedEventArgs e) => ShowLoginPanel("Administration employee");
        private void Employee_Click(object sender, RoutedEventArgs e) => ShowLoginPanel("Employee");
        private void Purchaser_Click(object sender, RoutedEventArgs e) => ShowLoginPanel("Purchaser");

        private void ShowLoginPanel(string role)
        {
            _selectedRole = role;
            SelectedRoleText.Text = $"Role: {role}";
            RoleSelectionPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RoleSelectionPanel.Visibility = Visibility.Visible;
            _selectedRole = string.Empty;
            LoginBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
        }

        //private void Login_Click(object sender, RoutedEventArgs e)
        //{
        //    // Здесь позже добавится логика проверки логина/пароля через API
        //    if (string.IsNullOrWhiteSpace(LoginBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
        //    {
        //        MessageBox.Show("Please enter both login and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    else
        //    {
        //        switch (_selectedRole)
        //        {
        //            case "Administrator":
        //                new AdminMainView().Show();
        //                break;
        //            case "Administration employee":
        //                new AdminUserMainView().Show();
        //                break;
        //            case "Employee":
        //                new EmployeeMainView().Show();
        //                break;
        //            case "Purchaser":
        //                new PurchaserMainView().Show();
        //                break;
        //        }
        //    }
        //    //MessageBox.Show($"Login successful as {_selectedRole}!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    Close();
        //}
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Please enter both login and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Показываем индикатор загрузки
            //LoginButton.IsEnabled = false;
            //LoginButton.Content = "Logging in...";

            try
            {
                // Создаем запрос для API
                var loginRequest = new LoginRequest
                {
                    Login = LoginBox.Text,
                    Password = PasswordBox.Password
                };

                // Отправляем запрос к API
                var response = await _apiClient.PostResponseAsync<LoginRequest, LoginResponse>(
                    loginRequest, "authorization/login");

                if (response != null)
                {
                    // Авторизация успешна
                    OpenRoleSpecificWindow(_selectedRole);
                    Close();
                }
                else
                {

                    MessageBox.Show("", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Восстанавливаем кнопку
                //LoginButton.IsEnabled = true;
                //LoginButton.Content = "Login";
            }
        }
        private void OpenRoleSpecificWindow(string role)
        {
            switch (role)
            {
                case "Administrator":
                    new AdminMainView().Show();
                    break;
                case "Administration employee":
                    new AdminUserMainView().Show();
                    break;
                case "Employee":
                    new EmployeeMainView().Show();
                    break;
                case "Purchaser":
                    new PurchaserMainView().Show();
                    break;
                default:
                    MessageBox.Show("Unknown role selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void mainWindowClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
