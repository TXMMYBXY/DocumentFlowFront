using DocumentFlowing.Client.Admin.Dtos;

namespace DocumentFlowing.Interfaces.Client;

public interface IAdminClient
{
    Task<List<GetUserDto>> GetAllUsersAsync(string uri);
    Task<object> CreateNewUserAsync(CreateNewUserDto createNewUserDto);
    Task<bool> ChangeStatusByIdAsync(int userId);
    Task DeleteUserByIdAsync(int selectedUserId);
    Task ChangePasswordByIdAsync(int userId, ResetPasswordDto resetPasswordDto);
}