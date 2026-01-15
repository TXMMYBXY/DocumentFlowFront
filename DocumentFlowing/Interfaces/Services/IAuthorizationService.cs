namespace DocumentFlowing.Interfaces.Services;

public interface IAuthorizationService
{
    /// <summary>
    /// Метод для логина по рефреш-токену
    /// </summary>
    /// <returns>допуск/не допуск</returns>
    Task<bool> TryAutoLoginAsync();
    
    /// <summary>
    /// Метод для логина по почте и паролю
    /// </summary>
    /// <returns>ID роли пользователя</returns>
    Task<int?> LoginAsync(string email, string password);
}