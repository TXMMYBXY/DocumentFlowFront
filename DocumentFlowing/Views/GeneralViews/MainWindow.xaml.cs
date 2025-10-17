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
        public MainWindow()
        {
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
            new LoginView().Show();
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