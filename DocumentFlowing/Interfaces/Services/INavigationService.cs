using DocumentFlowing.ViewModels.Base;
using System.Windows;

namespace DocumentFlowing.Interfaces.Services;

public interface INavigationService
{
    /// <summary>
    /// Метод для открытия окна по роли пользователя 
    /// </summary>
    void NavigateToRole(int? roleId);
    
    /// <summary>
    /// Метод для открытия модального окна
    /// </summary>
    /// <typeparam name="TView">Окно</typeparam>
    void NavigateTo<TView>() where TView : Window;
    
    /// <summary>
    /// Метод для закрытия текущего окна
    /// </summary>
    void CloseCurrentWindow();
    
    /// <summary>
    /// Метод открывает немодальное окно
    /// </summary>
    /// <typeparam name="T">Окно</typeparam>
    bool? ShowDialog<T>() where T : Window;
    /// <summary>
    /// Метод открывает немодальное окно
    /// </summary>
    /// <param name="viewModel">ViewModel, в которую нужно передать параметры</param>
    /// <typeparam name="TView">View</typeparam>
    /// <returns></returns>
    bool? ShowDialog<T>(BaseViewModel viewModel) where T : Window;
}