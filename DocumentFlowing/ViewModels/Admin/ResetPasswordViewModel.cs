using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using DocumentFlowing.ViewModels.Base;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Admin;

public class ResetPasswordViewModel : BaseViewModel, IDialogService
{
    private readonly ResetPasswordModel _resetPasswordModel;
    private string _password;
    
    public event Action<bool>? DialogClosed;

    public ResetPasswordViewModel(IAdminClient adminClient, int userId)
    {
        _resetPasswordModel = new ResetPasswordModel(adminClient);

        ConfirmCommand = new RelayCommand(async () => _ChangePassword(userId),()=> !string.IsNullOrWhiteSpace(Password));
        CancelCommand = new RelayCommand(() => DialogClosed?.Invoke(true));
    }
    
    public ICommand CancelCommand { get; private set; }
    public ICommand ConfirmCommand { get; private set; }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    private async Task _ChangePassword(int userId)
    {
        await _resetPasswordModel.ChangePassword(userId, Password);
        
        DialogClosed?.Invoke(true);
    }
}