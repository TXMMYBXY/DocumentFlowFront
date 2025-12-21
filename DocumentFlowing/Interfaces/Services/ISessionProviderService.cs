namespace DocumentFlowing.Interfaces.Services;

public interface ISessionProviderService
{
    Task LogoutAsync();
    bool IsAuthenticated { get; }
    event EventHandler LogoutRequested;
}