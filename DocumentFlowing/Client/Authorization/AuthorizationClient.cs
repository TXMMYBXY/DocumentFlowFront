using DocumentFlowing.Client.Authorization.Dtos;
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

    public async Task<RefreshTokenToLoginResponseDto> RequestForAccessAsync(RefreshTokenToLoginRequestDto request, string uri)
    {
        return await PostResponseAsync<RefreshTokenToLoginRequestDto, RefreshTokenToLoginResponseDto>(request, uri);
    }

    public async Task<AccessTokenResponseDto> GetNewAccessTokenAsync(AccessTokenRequestDto requestDto, string uri)
    {
        return await PostResponseAsync<AccessTokenRequestDto, AccessTokenResponseDto>(requestDto, uri);
    }

    public async Task<RefreshTokenResponseDto> GetNewRefreshTokenAsync(RefreshTokenRequestDto requestDto, string uri)
    {
        return await PostResponseAsync<RefreshTokenRequestDto, RefreshTokenResponseDto>(requestDto, uri);
    }
}