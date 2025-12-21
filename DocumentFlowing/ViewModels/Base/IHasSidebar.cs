using DocumentFlowing.ViewModels.Controls;

namespace DocumentFlowing.ViewModels.Base;

public interface IHasSidebar
{
    SidebarViewModel SidebarViewModel { get; set; }
}