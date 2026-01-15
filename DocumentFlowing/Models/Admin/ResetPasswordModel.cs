using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Interfaces.Client;

namespace DocumentFlowing.Models.Admin;

public class ResetPasswordModel
{
    private readonly IAdminClient _adminClient;
    
    public ResetPasswordModel(IAdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task ChangePassword(int userId, string password)
    {
        await _adminClient.ChangePasswordByIdAsync(userId, new ResetPasswordDto{Password = password});
    }
}