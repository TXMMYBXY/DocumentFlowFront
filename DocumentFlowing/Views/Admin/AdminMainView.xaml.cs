using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Controls;
using DocumentFlowing.Views.Authorization;
using DocumentFlowing.Views.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DocumentFlowing.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainView : Window
    {
        public AdminMainView()
        {
            InitializeComponent();
        }
    }
}
