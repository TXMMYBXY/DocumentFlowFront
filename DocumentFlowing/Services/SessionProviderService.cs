using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DocumentFlowing.Services;

public class SessionProviderService : ISessionProviderService
{
    private readonly ITokenService _tokenService;
    private readonly INavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;

    public SessionProviderService(ITokenService tokenService, INavigationService navigationService)
    {
        _tokenService = tokenService;
        _navigationService = navigationService;
    }
    
    public async Task LogoutAsync()
    {
        // 1. Очищаем токены
        _tokenService.ClearTokens();
        
        // 2. Оповещаем подписчиков о выходе
        LogoutRequested?.Invoke(this, EventArgs.Empty);
        
        // 3. Навигация на окно логина
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            // Закрываем текущее главное окно
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }
            
            // Открываем новое окно логина
            var loginView = _serviceProvider.GetService<LoginView>();
            loginView.Show();
            
            // Устанавливаем его как главное
            Application.Current.MainWindow = loginView;
        });
    }

    public bool IsAuthenticated => _tokenService.IsRefreshTokenValid();
    public event EventHandler? LogoutRequested;
}