using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.ViewModels.Admin;
using DocumentFlowing.Views.Admin;

namespace DocumentFlowing.Models;

public class UserModel
{
    private readonly IAdminClient _adminClient;
    private readonly INavigationService _navigationService;
    private List<GetUserDto> _users;
    
    public UserModel(IAdminClient adminClient, INavigationService navigationService)
    {
        _adminClient = adminClient;
        _navigationService = navigationService;
    }
    
    public async Task<List<GetUserDto>> GetAllUsersAsync()
    {
        _users = await _adminClient.GetAllUsersAsync("users/get-all");
        return _users;
    }

    public void OpenModalWindowCreateUser()
    {
        _navigationService.ShowDialog<CreateUserView>();
    }

    public async Task<bool> ChangeStatusByIdAsync(int userId)
    {
        return await _adminClient.ChangeStatusByIdAsync(userId);
    }

    public async Task DeleteUserByIdAsync(int selectedUserId)
    {
        await _adminClient.DeleteUserByIdAsync(selectedUserId);
    }

    public void OpenModalWindowChangePassword(int userId)
    {
        var resetPasswordViewModel = new ResetPasswordViewModel(_adminClient, userId);

        _navigationService.ShowDialog<ResetPasswordView>(resetPasswordViewModel);
    }
}