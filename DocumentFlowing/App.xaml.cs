using DocumentFlowing.Configuration;
using DocumentFlowing.Helpers;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Authorization;
using DocumentFlowing.Views.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DocumentFlowing;

public partial class App : Application
{
    private IServiceProvider _serviceProvider;
    
    public App()
    {
        var services = new ServiceCollection();
        
        var configuration = ConfigurationHelper.Configuration;
        
        services.AddAppServices(configuration);
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        var services = new ServiceCollection();
        var configuration = ConfigurationHelper.Configuration;
        
        services.AddAppServices(configuration);
        
        _serviceProvider = services.BuildServiceProvider();
        
        // Проверяем токен и показываем соответствующее окно
        using (var scope = _serviceProvider.CreateScope())
        {
            var tokenService = scope.ServiceProvider.GetService<ITokenService>();
            var navigationService = scope.ServiceProvider.GetService<INavigationService>();
            
            if (tokenService != null && tokenService.IsRefreshTokenValid())
            {
                var userInfo = tokenService.GetUserInfo();
                if (userInfo != null)
                {
                    navigationService.NavigateToRole(userInfo.RoleId);
                    return;
                }
            }
            
            navigationService.NavigateTo<LoginView>();
        }
    }
    
    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}