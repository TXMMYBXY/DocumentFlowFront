namespace DocumentFlowing.Interfaces.Client;

public interface IGeneralClient
{
    Task<TResponse?> UpdateResponseAsync<TRequest, TResponse>(TRequest request, string uri);
    Task<TResponse?> PostResponseAsync<TRequest, TResponse>(TRequest request, string uri);
    Task<TResponse?> DeleteResponseAsync<TRequest, TResponse>(TRequest request, string uri);
    Task<TResponse?> GetResponseAsync<TResponse>(string uri);
}