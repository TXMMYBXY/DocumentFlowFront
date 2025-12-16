using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views;
using DocumentFlowing.Views.Admin;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAuthorizationClient _authorizationClient;
        private readonly ITokenService _tokenService;
        public MainWindow(IAuthorizationClient authorizationClient,  ITokenService tokenService)
        {
            _authorizationClient = authorizationClient;
            _tokenService = tokenService;
            InitializeComponent();
            ContentArea.Content = new TextBlock
            {
                Text = "Wellcome to DocumentFlowing!",
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }

        private void loginClick(object sender, RoutedEventArgs e)
        {
            new LoginWindow(_authorizationClient, _tokenService).Show();
            Close();
        }

        private void settingsClick(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new SettingsView();
        }

        private void helpClick(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new TextBlock
            {
                Text = "Please contact to your system-admin!",
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
                
        }
    }
}