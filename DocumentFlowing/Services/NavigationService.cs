using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Admin;
using DocumentFlowing.ViewModels.Authorization;
using DocumentFlowing.ViewModels.Base;
using DocumentFlowing.ViewModels.Boss;
using DocumentFlowing.ViewModels.Controls;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Authorization;
using DocumentFlowing.Views.Boss;
using DocumentFlowing.Views.Controls;
using DocumentFlowing.Views.Purchaser;
using DocumentFlowing.Views.User;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

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
            { typeof(CreateUserView), typeof(CreateUserViewModel) },
            { typeof(ContractTemplateView), typeof(ContractTemplateViewModel) }
        };
        
        // Инициализаторы для View с SideBar
        _viewInitializers = new Dictionary<Type, Action<Window, IServiceProvider>>
        {
            { typeof(AdminMainView), _InitializeWindowWithSidebar },
            { typeof(BossMainView), _InitializeWindowWithSidebar },
            { typeof(PurchaserMainView), _InitializeWindowWithSidebar },
            { typeof(UserMainView), _InitializeWindowWithSidebar },
            { typeof(ContractTemplateView), _InitializeWindowWithSidebar }
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

    public void NavigateTo<TView>() where TView : Window
    {
        var scope = _serviceProvider.CreateScope();
        
        var view = (TView)scope.ServiceProvider.GetService(typeof(TView));
        
        if (view == null)
        {
            scope.Dispose();
            throw new InvalidOperationException($"View {typeof(TView)} not registered");
        }

        view.Closed += (s, e) => scope.Dispose();

        if (_viewToViewModelMapping.TryGetValue(typeof(TView), out var viewModelType))
        {
            var viewModel = scope.ServiceProvider.GetService(viewModelType) as BaseViewModel;
            view.DataContext = viewModel;
        }
        
        if (_viewInitializers.TryGetValue(typeof(TView), out var initializer))
        {
            initializer(view, scope.ServiceProvider);
        }
        
        _ShowWindow(view);
    }

    public bool? ShowDialog<T>() where T : Window
    {
        var scope = _serviceProvider.CreateScope();
        
        try
        {
            var dialog = scope.ServiceProvider.GetService(typeof(T)) as Window;
            
            if (dialog == null)
            {
                scope.Dispose();
                throw new InvalidOperationException($"Dialog {typeof(T)} not registered");
            }

            if (_viewToViewModelMapping.TryGetValue(typeof(T), out var viewModelType))
            {
                var viewModel = scope.ServiceProvider.GetService(viewModelType) as BaseViewModel;
                
                if (viewModel != null)
                {
                    dialog.DataContext = viewModel;

                    if (viewModel is IDialogService dialogViewModel)
                    {
                        dialogViewModel.DialogClosed += _ => dialog.Close();
                    }
                }
            }

            dialog.Closed += (s, e) => scope.Dispose();
            dialog.Owner = Application.Current.MainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            
            return dialog.ShowDialog();
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
    
    public bool? ShowDialog<T>(BaseViewModel viewModel) where T : Window
    {
        var scope = _serviceProvider.CreateScope();

        try
        {
            var dialog = scope.ServiceProvider.GetService(typeof(T)) as Window;
            if (dialog == null)
            {
                throw new InvalidOperationException($"Dialog {typeof(T)} not registered");
            }

            dialog.DataContext = viewModel;

            if (viewModel is IDialogService dialogViewModel)
            {
                dialogViewModel.DialogClosed += _ => dialog.Close();
            }

            dialog.Owner = Application.Current.MainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Closed += (s, e) => scope.Dispose();

            return dialog.ShowDialog();
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
        
    private void _InitializeWindowWithSidebar(Window window, IServiceProvider serviceProvider)
    {
        if (window.DataContext is IHasSidebar sidebarContainer)
        {
            var sidebarViewModel = serviceProvider.GetService<SidebarViewModel>();
            sidebarContainer.SidebarViewModel = sidebarViewModel;
            
            _ConfigureSidebarForRole(sidebarViewModel, sidebarContainer);
        }
    }
    
    private void _ShowWindow(Window newWindow)
    {
        var currentWindow = Application.Current.MainWindow;
        
        newWindow.Show();
        Application.Current.MainWindow = newWindow;
        
        if (currentWindow != null && currentWindow != newWindow)
        {
            currentWindow.Close();
        }
    }
    
    private void _ConfigureSidebarForRole(SidebarViewModel sidebarViewModel, IHasSidebar mainViewModel)
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