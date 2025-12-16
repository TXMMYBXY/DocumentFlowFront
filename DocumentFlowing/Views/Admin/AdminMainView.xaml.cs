using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using System.Windows;

namespace DocumentFlowing.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainView : Window
    {
        private readonly IAuthorizationClient _authorizationClient;
        private readonly ITokenService _tokenService;
        public AdminMainView(IAuthorizationClient authorizationClient,  ITokenService tokenService)
        {
            _authorizationClient = authorizationClient;
            _tokenService = tokenService;
            InitializeComponent();
            ContentArea.Content = new TemplatesView();
        }

        private void templates_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new TemplatesView();
        }

        private void users_Click(object sernder, RoutedEventArgs e)
        {
            ContentArea.Content = new UsersView();
        }

        private void Sidebar_LogoutClicked(object sender, RoutedEventArgs e)
        {
            new LoginWindow(_authorizationClient, _tokenService).Show();
            Close();
        }
        private void Sidebar_SettingsClicked(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new SettingsView();
        }
    }
}
