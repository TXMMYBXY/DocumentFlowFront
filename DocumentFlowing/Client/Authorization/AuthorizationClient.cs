using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace DocumentFlowing.Client.Authorization;

public class AuthorizationClient : GeneralClient, IAuthorizationClient
{
    
    public AuthorizationClient(HttpClient httpClient, IOptions<DocumentFlowApi> documentFlowApi) : base(httpClient, documentFlowApi)
    {
    }

    public async Task<TResponse> LoginAsync<TRequest, TResponse>(TRequest request, string uri)
    {
        return await PostResponseAsync<TRequest, TResponse>(request, uri);
    }

    public async Task<TResponse> RequestForAccessAsync<TRequest, TResponse>(TRequest request, string uri)
    {
        return await PostResponseAsync<TRequest, TResponse>(request, uri);
    }
}