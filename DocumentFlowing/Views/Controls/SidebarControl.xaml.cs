using DocumentFlowing.ViewModels.Authorization;
using DocumentFlowing.ViewModels.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DocumentFlowing.Views.Controls
{
    public partial class SidebarControl : UserControl
    {
        public SidebarControl()
        {
            InitializeComponent();

            DataContext = new ControlsViewModel();
        }
    }
}
