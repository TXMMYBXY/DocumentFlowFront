using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Authorization;
using DocumentFlowing.Views.Controls;
using System.Windows;

namespace DocumentFlowing.Views.Boss
{
    /// <summary>
    /// Логика взаимодействия для AdminUserMainView.xaml
    /// </summary>
    public partial class BossMainView : Window
    {

        public BossMainView()
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
    }
}
