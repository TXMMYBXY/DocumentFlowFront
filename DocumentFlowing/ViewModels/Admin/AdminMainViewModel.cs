using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Base;
using System.ComponentModel;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Admin
{
    public class AdminMainViewModel : MainViewModelBase
    {
        private object _currentView;
        private readonly INavigationService _navigationService;
        
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
