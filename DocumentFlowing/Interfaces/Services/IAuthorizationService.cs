using DocumentFlowing.Client.Authorization.ViewModels;
using DocumentFlowing.Models;

namespace DocumentFlowing.Interfaces.Services;

public interface IAuthorizationService
{
    Task<bool> TryAutoLoginAsync();
    Task<int?> LoginAsync(string email, string password);
}