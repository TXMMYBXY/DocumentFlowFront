using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Authorization;
using System.Windows;

namespace DocumentFlowing.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainView : Window
    {
        public AdminMainView()
        {
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
            
        }
        private void Sidebar_SettingsClicked(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new SettingsView();
        }
    }
}
