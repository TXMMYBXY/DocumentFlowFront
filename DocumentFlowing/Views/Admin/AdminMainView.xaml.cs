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

        private void settings_Click(object sernder, RoutedEventArgs e)
        {
            ContentArea.Content = new SettingsView();
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            new LoginView().Show();
            Close();
        }
    }
}
