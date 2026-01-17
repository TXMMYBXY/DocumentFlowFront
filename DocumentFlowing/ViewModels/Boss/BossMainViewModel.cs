using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Base;
using DocumentFlowing.ViewModels.Controls;
using DocumentFlowing.Views.Controls;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Boss;

public class BossMainViewModel : MainViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IBossClient _bossClient;
    private readonly ISessionProviderService _sessionProvider;
    
    private object _currentView;
    
    public object CurrentView
    {
        get => _currentView;
        set => SetField(ref _currentView, value);
    }
    
    public ICommand ShowContractsCommand { get; set; }
    public ICommand ShowReportsCommand { get; set; }
    public ICommand ShowArchiveCommand { get; set; }
    
    public BossMainViewModel(
        IBossClient bossClient, 
        INavigationService navigationService, 
        ISessionProviderService sessionProvider)
    {
        _bossClient = bossClient;
        _navigationService = navigationService;
        _sessionProvider = sessionProvider;
        
        ShowContractsCommand = new RelayCommand(_ShowTemplates);
        ShowReportsCommand = new RelayCommand(() => throw new NotImplementedException());
        ShowArchiveCommand = new RelayCommand(() => throw new NotImplementedException());
        
        _ShowTemplates();
    }

    private void _ShowTemplates()
    {
        var templatesView = new TemplateView();
        templatesView.DataContext = new TemplateViewModel(_bossClient, _navigationService, _sessionProvider);
        CurrentView = templatesView;
    }
}