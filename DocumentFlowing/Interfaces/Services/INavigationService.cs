namespace DocumentFlowing.Interfaces.Services;

public interface INavigationService
{
    /// <summary>
    /// Метод для открытия окна по роли пользователя 
    /// </summary>
    void NavigateToRole(int roleId);
}