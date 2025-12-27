using DocumentFlowing.Client.Admin.ViewModels;

namespace DocumentFlowing.Interfaces.Client;

public interface IAdminClient
{
    Task<List<GetUserViewModel>> GetAllUsersAsync(string uri);
}