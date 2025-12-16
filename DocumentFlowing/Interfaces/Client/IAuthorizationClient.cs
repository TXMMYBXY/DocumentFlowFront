namespace DocumentFlowing.Interfaces.Client;

public interface IAuthorizationClient
{
    Task<TResponse> LoginAsync<TRequest, TResponse>(TRequest request, string uri);
}