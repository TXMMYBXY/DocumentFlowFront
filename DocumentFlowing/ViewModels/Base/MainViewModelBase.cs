using DocumentFlowing.Services;
using DocumentFlowing.ViewModels.Controls;

namespace DocumentFlowing.ViewModels.Base;

public abstract class MainViewModelBase : BaseViewModel, IHasSidebar
{
    private SidebarViewModel _sidebarViewModel;
    
    public SidebarViewModel SidebarViewModel
    {
        get => _sidebarViewModel;
        set => SetField(ref _sidebarViewModel, value);
    }
    
    protected MainViewModelBase()
    {
        // Общие команды для всех главных окон
        // ...
    }
}