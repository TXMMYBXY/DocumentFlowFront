using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Base;
using DocumentFlowing.ViewModels.Controls;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Admin;

public class AdminMainViewModel : MainViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IAdminClient _adminClient;
        
    private object _currentView;
        
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
            
        ShowTemplatesCommand = new RelayCommand(_ShowTemplates);
        ShowUsersCommand = new RelayCommand(_ShowUsers);
            
        _ShowUsers();
    }
        
    private void _ShowTemplates()
    {
        // var templatesView = new Views.Controls.TemplatesView();
        // // Установите DataContext если нужно
        // // templatesView.DataContext = new TemplatesViewModel();
        // CurrentView = templatesView;
    }
        
    private void _ShowUsers()
    {
        var usersView = new Views.Controls.UserView();
        usersView.DataContext = new UserViewModel(_adminClient, _navigationService);
        CurrentView = usersView;
    }
}