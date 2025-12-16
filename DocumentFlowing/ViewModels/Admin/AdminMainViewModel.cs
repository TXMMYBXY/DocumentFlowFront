using System.ComponentModel;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Admin
{
    public class AdminMainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public ICommand ShowTemplatesCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public AdminMainViewModel()
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
