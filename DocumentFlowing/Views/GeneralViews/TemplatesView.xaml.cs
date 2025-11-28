using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для TemplatesView.xaml
    /// </summary>
    public partial class TemplatesView : UserControl
    {
        private bool _isPreviewVisible = true;

        public TemplatesView()
        {
            InitializeComponent();

            // Пример данных
            TemplateList.ItemsSource = new List<TemplateInfo>
            {
                new TemplateInfo { Name = "Договор поставки", Created = "2025-01-12", Description = "Стандартная форма договора с поставщиком." },
                new TemplateInfo { Name = "Акт приёма-передачи", Created = "2025-02-05", Description = "Документ для подтверждения передачи имущества." },
                new TemplateInfo { Name = "Заявка на закупку", Created = "2025-03-10", Description = "Форма для подачи заявки на приобретение товара или услуги." }
            };
            PreviewColumn.Width = new GridLength(0);
            _isPreviewVisible = false;
            TogglePreviewButton.Content = "<<";
        }

        private void addTemplate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавление шаблона (функционал пока не реализован)");
        }

        private void TemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemplateList.SelectedItem is TemplateInfo selected)
            {
                PreviewTitle.Text = selected.Name;
                PreviewDescription.Text = selected.Description;
            }
        }

        private void TogglePreview_Click(object sender, RoutedEventArgs e)
        {
            if (_isPreviewVisible)
            {
                PreviewColumn.Width = new GridLength(0);
                TogglePreviewButton.Content = "<<";
            }
            else
            {
                PreviewColumn.Width = new GridLength(3, GridUnitType.Star);
                TogglePreviewButton.Content = ">>";
            }

            _isPreviewVisible = !_isPreviewVisible;
        }

        private class TemplateInfo
        {
            public string Name { get; set; }
            public string Created { get; set; }
            public string Description { get; set; }
        }
    }
}
