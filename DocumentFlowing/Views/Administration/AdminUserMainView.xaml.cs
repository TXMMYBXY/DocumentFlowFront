using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Controls;
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
            new LoginView().Show();
            Close();
        }
        private void Sidebar_SettingsClicked(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new SettingsView();
        }
    }
}
