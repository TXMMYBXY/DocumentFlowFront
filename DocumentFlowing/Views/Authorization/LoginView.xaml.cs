using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Authorization;
using System.Windows;

namespace DocumentFlowing.Views.Authorization;

public partial class LoginView : Window
{
    public event EventHandler LoginSuccessful;
    
    public LoginView()
    {
        InitializeComponent();
    }
}