using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Client.Authorization.ViewModels;
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

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string uri)
    {
        return await PostResponseAsync<LoginRequestDto, LoginResponseDto>(request, uri);
    }

    public async Task<RefreshTokenToLoginResponseViewModel> RequestForAccessAsync(RefreshTokenToLoginRequestViewModel request, string uri)
    {
        return await PostResponseAsync<RefreshTokenToLoginRequestViewModel, RefreshTokenToLoginResponseViewModel>(request, uri);
    }

    public async Task<AccessTokenViewModelResponse> GetNewAccessTokenAsync(AccessTokenViewModelRequest request, string uri)
    {
        return await PostResponseAsync<AccessTokenViewModelRequest, AccessTokenViewModelResponse>(request, uri);
    }

    public async Task<RefreshTokenResponseViewModel> GetNewRefreshTokenAsync(RefreshTokenViewModelRequest request, string uri)
    {
        return await PostResponseAsync<RefreshTokenViewModelRequest, RefreshTokenResponseViewModel>(request, uri);
    }
}