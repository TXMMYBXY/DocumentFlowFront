using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Controls;
using System.Windows;

namespace DocumentFlowing.Views.Boss
{
    /// <summary>
    /// Логика взаимодействия для AdminUserMainView.xaml
    /// </summary>
    public partial class BossMainView : Window
    {
        private readonly IAuthorizationClient _authorizationClient; 
        private readonly ITokenService _tokenService;
        public BossMainView(IAuthorizationClient authorizationClient,  ITokenService tokenService)
        {
            _authorizationClient = authorizationClient;
            _tokenService = tokenService;
            InitializeComponent();
            ContentArea.Content = new TemplatesView();
            //Sidebar.AddMenuItem("Documents", documents_Click);
            //Sidebar.AddMenuItem("Reports", reports_Click);
        }

        private void documents_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("documentsClick");
        }

        private void reports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("reportsClick");
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
