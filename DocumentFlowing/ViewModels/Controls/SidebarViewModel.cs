using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Controls;

public class SidebarViewModel : BaseViewModel
{
    private readonly ISessionProviderService _sessionProviderService;

    public ICommand SettingsCommand { get; }
    public ICommand LogoutCommand { get; set; }
    
    public SidebarViewModel(ISessionProviderService sessionProviderService)
    {
        _sessionProviderService = sessionProviderService;
        
        LogoutCommand = new RelayCommand(async () => await _ExecuteLogoutAsync());
    }
    
    private async Task _ExecuteLogoutAsync()
    {
        var result = MessageBox.Show(
            "Вы уверены, что хотите выйти?", 
            "Подтверждение выхода", 
            MessageBoxButton.YesNo, 
            MessageBoxImage.Question);
            
        if (result == MessageBoxResult.Yes)
        {
            await _sessionProviderService.LogoutAsync();
        }
    }
    
}