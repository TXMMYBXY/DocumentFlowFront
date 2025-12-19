using DocumentFlowing.Configuration;
using DocumentFlowing.Helpers;
using DocumentFlowing.Interfaces.Services;
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
            
        try
        {
            var loginView = _serviceProvider.GetService<LoginView>();
            var viewModel = loginView.DataContext as IAsyncInitialization;
                
            if (loginView != null)
            {
                MainWindow = loginView;
                loginView.Show();
                
                await viewModel.Initialization;
            }
            else
            {
                MessageBox.Show("Не удалось создать окно авторизации", 
                    "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при запуске приложения: {ex.Message}", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }
    
    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}