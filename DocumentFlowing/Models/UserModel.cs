using DocumentFlowing.Client.Admin.ViewModels;
using DocumentFlowing.Interfaces.Client;

namespace DocumentFlowing.Models;

public class UserModel
{
    private readonly IAdminClient _adminClient;
    public List<GetUserViewModel> Users;
    
    public UserModel(IAdminClient adminClient)
    {
        _adminClient = adminClient;
    }
    
    public string FullName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
    
    public async Task<List<GetUserViewModel>> GetAllUsersAsync()
    {
        Users = await _adminClient.GetAllUsersAsync("users/get-all");
        return Users;
    }
}