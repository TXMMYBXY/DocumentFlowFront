using DocumentFlowing.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Controls.Items
{
    public class MenuItemViewModel : BaseViewModel
    {
        private Visibility _visibility = Visibility.Visible;

        public Visibility Visibility
        {
            get => _visibility;
            set => SetField(ref _visibility, value);
        }
        public string Header { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
    }
}