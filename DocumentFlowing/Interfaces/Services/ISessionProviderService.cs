namespace DocumentFlowing.Interfaces.Services;

public interface ISessionProviderService
{
    /// <summary>
    /// Метод для очистки токенов и открытия окна авторизации
    /// </summary>
    Task LogoutAsync();
    bool IsAuthenticated { get; }
    event EventHandler LogoutRequested;
}