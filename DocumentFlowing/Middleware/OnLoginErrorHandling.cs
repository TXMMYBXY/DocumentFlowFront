using DocumentFlowing.Interfaces.Services;
using System.Windows;
using System.Windows.Threading;

namespace DocumentFlowing.Middleware;

public class OnLoginErrorHandling
{
    private readonly IAuthorizationService _authorizationService;
    
    public OnLoginErrorHandling()
    {
        
    }
    public void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        HandleException(e.Exception);
        e.Handled = true; // Исключение обработано, приложение не упадет
    }
    
    private void HandleException(Exception ex)
    {
        // Обрабатываем наше специальное исключение для 401 ошибок
        if (ex is UnauthorizedAccessException)
        {
            // Показываем сообщение (опционально)
            MessageBox.Show("Ваша сессия истекла. Пожалуйста, войдите снова.", 
                "Требуется авторизация", 
                MessageBoxButton.OK, 
                MessageBoxImage.Warning);
            
            // Перенаправляем на окно логина
            _authorizationService.TryAutoLoginAsync();
        }

    }
}