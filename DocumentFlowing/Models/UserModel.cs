using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Admin;

namespace DocumentFlowing.Models;

public class UserModel
{
    private readonly IAdminClient _adminClient;
    private readonly INavigationService _navigationService;
    public List<GetUserDto> Users;
    
    public UserModel(IAdminClient adminClient, INavigationService navigationService)
    {
        _adminClient = adminClient;
        _navigationService = navigationService;
    }
    
    public string FullName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
    
    public async Task<List<GetUserDto>> GetAllUsersAsync()
    {
        Users = await _adminClient.GetAllUsersAsync("users/get-all");
        return Users;
    }

    public async Task CreateNewUserAsync(CreateNewUserDto createNewUserDto)
    {
        
    }

    public void OpenModalWindowCreateUser()
    {
        _navigationService.ShowDialog<CreateUserView>();
    }
    
}