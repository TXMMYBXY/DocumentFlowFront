using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Controls;

public class SidebarViewModel : BaseViewModel
{
    private readonly ISessionProviderService _sessionProviderService;
    
    public SidebarViewModel(ISessionProviderService sessionProviderService)
    {
        _sessionProviderService = sessionProviderService;
        
        LogoutCommand = new RelayCommand(async () => await ExecuteLogoutAsync());
    }

    public ICommand SettingsCommand { get; }
    public ICommand LogoutCommand { get; set; }
    
    private async Task ExecuteLogoutAsync()
    {
        // Можно добавить подтверждение
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