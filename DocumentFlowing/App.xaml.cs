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
            
        // try
        // {
        //     // using (var scope = _serviceProvider.CreateScope())
        //     // {
        //     //     var tokenService = scope.ServiceProvider.GetService<ITokenService>();
        //     //     var navigationService = scope.ServiceProvider.GetService<INavigationService>();
        //     //
        //     //     if (tokenService != null && tokenService.IsRefreshTokenValid())
        //     //     {
        //     //         // Автоматический вход по токену
        //     //         var userInfo = tokenService.GetUserInfo();
        //     //         if (userInfo != null)
        //     //         {
        //     //             navigationService.NavigateToRole(userInfo.RoleId);
        //     //             return;
        //     //         }
        //     //     }
        //     //
        //     //     // Если токена нет, показываем логин
        //     //     navigationService.NavigateTo<LoginView>();
        //     // }
        //     //BUG: тут поток блокируется
        //     var loginView = _serviceProvider.GetService<LoginView>();
        //     var viewModel = loginView.DataContext as IAsyncInitialization;
        //         
        //     if (loginView != null)
        //     {
        //         MainWindow = loginView;
        //         loginView.Show();
        //         
        //         await viewModel.Initialization;
        //     }
        //     else
        //     {
        //         MessageBox.Show("Не удалось создать окно авторизации", 
        //             "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Error);
        //         Shutdown();
        //     }
        // }
        // catch (Exception ex)
        // {
        //     MessageBox.Show($"Ошибка при запуске приложения: {ex.Message}", 
        //         "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //     Shutdown();
        // }
        
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