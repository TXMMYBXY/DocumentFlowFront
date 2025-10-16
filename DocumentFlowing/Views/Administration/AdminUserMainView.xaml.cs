using System.Windows;

namespace DocumentFlowing.Views.Administration
{
    /// <summary>
    /// Логика взаимодействия для AdminUserMainView.xaml
    /// </summary>
    public partial class AdminUserMainView : Window
    {
        public AdminUserMainView()
        {
            InitializeComponent();

            Sidebar.AddMenuItem("Documents", documents_Click);
            Sidebar.AddMenuItem("Reports", reports_Click);
        }

        private void documents_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settings_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Sidebar_LogoutClicked(object sender, RoutedEventArgs e)
        {
            new LoginView().Show();
            Close();
        }
        private void Sidebar_SettingsClicked(object sender, RoutedEventArgs e)
        {
            
            Close();
        }
    }
}
