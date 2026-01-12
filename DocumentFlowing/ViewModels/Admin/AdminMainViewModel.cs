using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Base;
using DocumentFlowing.ViewModels.Controls;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Admin
{
    public class AdminMainViewModel : MainViewModelBase
    {
        private object _currentView;
        private readonly INavigationService _navigationService;
        private readonly IAdminClient _adminClient;
        
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }
        
        public ICommand ShowTemplatesCommand { get; }
        public ICommand ShowUsersCommand { get; }
        
        public AdminMainViewModel(INavigationService navigationService, IAdminClient adminClient)
        {
            _navigationService = navigationService;
            _adminClient = adminClient;
            
            ShowTemplatesCommand = new RelayCommand(ShowTemplates);
            ShowUsersCommand = new RelayCommand(ShowUsers);
            
            // По умолчанию показываем шаблоны или пользователей
            ShowTemplates(); // Или ShowUsers();
        }
        
        private void ShowTemplates()
        {
            // var templatesView = new Views.Controls.TemplatesView();
            // // Установите DataContext если нужно
            // // templatesView.DataContext = new TemplatesViewModel();
            // CurrentView = templatesView;
        }
        
        private void ShowUsers()
        {
            var usersView = new Views.Controls.UserView();
            usersView.DataContext = new UserViewModel(_adminClient, _navigationService);
            CurrentView = usersView;
        }
    }
}