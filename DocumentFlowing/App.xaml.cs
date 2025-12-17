using DocumentFlowing.Client.Authorization;
using DocumentFlowing.Client.Authorization.Services;
using DocumentFlowing.Configuration;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Boss;
using DocumentFlowing.Views.Purchaser;
using DocumentFlowing.Views.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DocumentFlowing;

public partial class App : Application
{
    private readonly IHost _host;
    private Window _currentWindow;
    
    private readonly IAuthorizationClient _authorizationClient;
    private readonly ITokenService _tokenService;
    
    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: false);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddAppServices(context.Configuration);
            })
            .Build();
        _authorizationClient = _host.Services.GetRequiredService<IAuthorizationClient>();
        _tokenService = _host.Services.GetRequiredService<ITokenService>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var authClient = _host.Services.GetRequiredService<IServiceProvider>().GetService<IAuthorizationService>();
        
        // Пытаемся выполнить автоматический вход
        bool isAuthenticated = await authClient.TryAutoLoginAsync();
        
        if (isAuthenticated)
        {
            // Показываем главное окно
            _ShowMainWindow();
        }
        else
        {
            // Показываем окно входа
            _ShowLoginWindow();
        }
        
        // var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        // mainWindow.Show();
    }
    
    private void _ShowMainWindow()
    {
        var userInfo = _tokenService.GetUserInfo();
        if (userInfo != null)
        {
            _OpenRoleSpecificWindow(userInfo.RoleId);
        }
    }
    
    private void _OpenRoleSpecificWindow(int roleId)
    {
        switch (roleId)
        {
            case 1:
                MessageBox.Show("Admin");
                new AdminMainView(_authorizationClient, _tokenService).Show();
                break;
            case 2:
                MessageBox.Show("Boss");
                new BossMainView(_authorizationClient, _tokenService).Show();
                break;
            case 3:
                new PurchaserMainView().Show();
                break;
            case 4:
                new UserMainView().Show();
                break;
        }
    }
    
    private void _ShowLoginWindow()
    {
        var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
        
        // Подписываемся на событие успешного входа
        loginWindow.LoginSuccessful += _OnLoginSuccessful;
        
        loginWindow.Show();
        _currentWindow = loginWindow;
    }
    
    private void _OnLoginSuccessful(object sender, EventArgs e)
    {
        // Закрываем окно входа
        if (_currentWindow is LoginWindow loginWindow)
        {
            loginWindow.Close();
        }
        
        // Показываем главное окно
        _ShowMainWindow();
    }
    
    protected override async void OnExit(ExitEventArgs e)
    {
        if (_currentWindow is LoginWindow loginWindow)
        {
            loginWindow.LoginSuccessful -= _OnLoginSuccessful;
        }
        
        using (_host)
            await _host.StopAsync();

        base.OnExit(e);
    }
}