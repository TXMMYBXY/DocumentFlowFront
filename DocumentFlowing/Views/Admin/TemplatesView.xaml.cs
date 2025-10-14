using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для TemplatesView.xaml
    /// </summary>
    public partial class TemplatesView : UserControl
    {
        public TemplatesView()
        {
            InitializeComponent();
        }

        private void addTemplate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("addTemplate_Click");
        }
    }
}
