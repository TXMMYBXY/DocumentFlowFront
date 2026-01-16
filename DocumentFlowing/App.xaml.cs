using DocumentFlowing.Configuration;
using DocumentFlowing.Helpers;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Authorization;
using DocumentFlowing.Views.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
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
        //
        // // Глобальные обработчики исключений
        // DispatcherUnhandledException += App_DispatcherUnhandledException;
        // TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        
        var services = new ServiceCollection();
        var configuration = ConfigurationHelper.Configuration;
        
        services.AddAppServices(configuration);
        
        _serviceProvider = services.BuildServiceProvider();
        
        using (var scope = _serviceProvider.CreateScope())
        {
            var navigationService = scope.ServiceProvider.GetService<INavigationService>();
            
            navigationService.NavigateTo<LoginView>();
        }
    }
    
    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
    //
    // private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    // {
    //     // Логируем исключение
    //     Debug.WriteLine($"!!! UNHANDLED UI EXCEPTION: {e.Exception}");
    //
    //     // Показываем пользователю сообщение об ошибке
    //     ShowUnhandledExceptionDialog("Критическая ошибка UI", e.Exception);
    //
    //     // Помечаем исключение как обработанное, чтобы приложение не закрылось
    //     e.Handled = true;
    // }
    //
    // private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    // {
    //     // Логируем исключение
    //     Debug.WriteLine($"!!! UNHANDLED TASK EXCEPTION: {e.Exception}");
    //
    //     // Показываем пользователю сообщение об ошибке
    //     ShowUnhandledExceptionDialog("Критическая ошибка в фоновой задаче", e.Exception);
    //
    //     // Помечаем исключение как "увиденное", чтобы процесс не завершился
    //     e.SetObserved();
    // }
    //
    // private void ShowUnhandledExceptionDialog(string title, Exception ex)
    // {
    //     var errorMessage = $"Произошла непредвиденная ошибка:\n\n{ex.Message}\n\nРекомендуется перезапустить приложение.";
    //     MessageBox.Show(errorMessage, title, MessageBoxButton.OK, MessageBoxImage.Error);
    // }
}