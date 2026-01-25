using DocumentFlowing.Interfaces.Client.Services;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DocumentFlowing.Middleware;

public class AuthorizationHandler : DelegatingHandler
{
    private static readonly SemaphoreSlim _refreshLock = new (1, 1);
    private static bool _isRefreshing = false;
    
    private readonly ITokenService _tokenService;
        
    public AuthorizationHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
        
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _tokenService.ReturnAccessToken();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        var response =  await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return await _HandleUnauthorizedResponse(request, cancellationToken, response);
        }

        return response;
    }

    private async Task<HttpResponseMessage> _HandleUnauthorizedResponse(
        HttpRequestMessage originalRequest,
        CancellationToken cancellationToken,
        HttpResponseMessage unauthorizedResponse)
    {
        unauthorizedResponse.Dispose();
        
        await _refreshLock.WaitAsync(cancellationToken);
        try
        {
            if (_isRefreshing)
            {
                while (_isRefreshing)
                {
                    await Task.Delay(100, cancellationToken);
                }
            }
            else
            {
                _isRefreshing = true;
                try
                {
                    var refreshed = _tokenService.ReturnRefreshToken();
                    
                    if (string.IsNullOrEmpty(refreshed))
                    {
                        throw new UnauthorizedAccessException("Failed to refresh access token");
                    }
                }
                finally
                {
                    _isRefreshing = false;
                }
            }
        }
        finally
        {
            _refreshLock.Release();
        }
        
        var newToken = await _tokenService.GetNewAccessTokenAsync();
        
        if (string.IsNullOrEmpty(newToken))
        {
            throw new UnauthorizedAccessException("Access token is missing after refresh");
        }
        
        var newRequest = await _CloneRequestAsync(originalRequest);
        newRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        
        return await base.SendAsync(newRequest, cancellationToken);
    }
    
    private async Task<HttpRequestMessage> _CloneRequestAsync(HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage
        {
            Method = original.Method,
            RequestUri = original.RequestUri,
            Version = original.Version
        };
        
        if (original.Content != null)
        {
            var ms = new MemoryStream();
            await original.Content.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);
            
            foreach (var header in original.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
        
        foreach (var header in original.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        
        return clone;
    }
}