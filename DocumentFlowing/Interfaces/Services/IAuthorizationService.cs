using DocumentFlowing.Models;

namespace DocumentFlowing.Interfaces.Services;

public interface IAuthorizationService
{
    Task<bool> TryAutoLoginAsync();
    Task LoginAsync(string email, string password);

}