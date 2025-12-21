using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Admin;
using DocumentFlowing.ViewModels.Authorization;
using DocumentFlowing.ViewModels.Base;
using DocumentFlowing.ViewModels.Boss;
using DocumentFlowing.ViewModels.Controls;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Authorization;
using DocumentFlowing.Views.Boss;
using DocumentFlowing.Views.Purchaser;
using DocumentFlowing.Views.User;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, Type> _viewToViewModelMapping;
    private readonly Dictionary<Type, Action<Window, IServiceProvider>> _viewInitializers;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        // Маппинг View -> ViewModel
        _viewToViewModelMapping = new Dictionary<Type, Type>
        {
            { typeof(LoginView), typeof(LoginViewModel) },
            { typeof(AdminMainView), typeof(AdminMainViewModel) },
            { typeof(BossMainView), typeof(BossMainViewModel) },
        };
        
        // Инициализаторы для View с SideBar
        _viewInitializers = new Dictionary<Type, Action<Window, IServiceProvider>>
        {
            { typeof(AdminMainView), _InitializeWindowWithSidebar },
            { typeof(BossMainView), _InitializeWindowWithSidebar },
            { typeof(PurchaserMainView), _InitializeWindowWithSidebar },
            { typeof(UserMainView), _InitializeWindowWithSidebar }
        };
    }
    
    public void NavigateToRole(int? roleId)
    {
        switch (roleId)
        {
            case 1:
                NavigateTo<AdminMainView>();
                break;
            case 2:
                NavigateTo<BossMainView>();
                break;
            case 3:
                NavigateTo<PurchaserMainView>();
                break;
            case 4:
                NavigateTo<UserMainView>();
                break;
        }
    }

    private void ShowWindow(Window newWindow)
    {
        var currentWindow = Application.Current.MainWindow;
        
        newWindow.Show();
        Application.Current.MainWindow = newWindow;
        
        if (currentWindow != null && currentWindow != newWindow)
        {
            currentWindow.Close();
        }
    }

    public void NavigateTo<TView>() where TView : Window
    {
        // Убираем 'using', чтобы scope не уничтожался сразу после выхода из метода.
        // Scope должен жить столько же, сколько и созданное им окно.
        var scope = _serviceProvider.CreateScope();
        
        // 1. Получаем View
        var view = (TView)scope.ServiceProvider.GetService(typeof(TView));
        
        if (view == null)
        {
            scope.Dispose(); // Если View не создалась, сразу освобождаем ресурсы.
            throw new InvalidOperationException($"View {typeof(TView)} not registered");
        }

        // Подписываемся на событие закрытия окна, чтобы уничтожить scope.
        view.Closed += (s, e) => scope.Dispose();

        // 2. Получаем соответствующий ViewModel
        if (_viewToViewModelMapping.TryGetValue(typeof(TView), out var viewModelType))
        {
            var viewModel = scope.ServiceProvider.GetService(viewModelType) as BaseViewModel;
            view.DataContext = viewModel;
        }
        
        // 3. Вызываем специальную инициализацию если есть
        if (_viewInitializers.TryGetValue(typeof(TView), out var initializer))
        {
            initializer(view, scope.ServiceProvider);
        }
        
        // 4. Показываем окно
        ShowWindow(view);
    }
    
    private void _InitializeWindowWithSidebar(Window window, IServiceProvider serviceProvider)
    {
        if (window.DataContext is IHasSidebar sidebarContainer)
        {
            // Создаем SidebarViewModel и передаем в основной ViewModel
            var sidebarViewModel = serviceProvider.GetService<SidebarViewModel>();
            sidebarContainer.SidebarViewModel = sidebarViewModel;
            
            // Настраиваем Sidebar для конкретной роли
            ConfigureSidebarForRole(sidebarViewModel, sidebarContainer);
        }
    }
    
    
    public void CloseCurrentWindow()
    {
        Application.Current.MainWindow?.Close();
    }

    public bool? ShowDialog<T>() where T : Window
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dialog = (Window)scope.ServiceProvider.GetService(typeof(T));
            
            if (dialog != null)
            {
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                
                return dialog.ShowDialog();
            }
        }
        
        return null;
    }
    
    private void ConfigureSidebarForRole(SidebarViewModel sidebarViewModel, IHasSidebar mainViewModel)
    {
        // // Очищаем старые пункты меню
        // sidebarViewModel.MenuItems.Clear();
        //
        // // Базовые пункты для всех
        // sidebarViewModel.MenuItems.Add(new MenuItem
        // {
        //     Title = "Главная",
        //     Command = new RelayCommand(() => { /* Навигация на главную */ })
        // });
        //
        // // Добавляем специфичные пункты в зависимости от типа ViewModel
        // if (mainViewModel is AdminMainViewModel)
        // {
        //     sidebarViewModel.MenuItems.Add(new MenuItem
        //     {
        //         Title = "Пользователи",
        //         Command = new RelayCommand(() => { /* Открыть управление пользователями */ })
        //     });
        //     sidebarViewModel.MenuItems.Add(new MenuItem
        //     {
        //         Title = "Статистика",
        //         Command = new RelayCommand(() => { /* Открыть статистику */ })
        //     });
        // }
        // // ... для других ролей
    }
}