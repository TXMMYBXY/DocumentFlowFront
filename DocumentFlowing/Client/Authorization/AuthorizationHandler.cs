using DocumentFlowing.Interfaces.Client.Services;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DocumentFlowing.Client.Authorization;

public class AuthorizationHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;
    private static readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);
    private static bool _isRefreshing = false;
        
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
        
        // Блокируем, чтобы избежать одновременных обновлений токена
        await _refreshLock.WaitAsync(cancellationToken);
        try
        {
            // Проверяем, не обновляется ли уже токен в другом потоке
            if (_isRefreshing)
            {
                // Ждем завершения обновления
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
                    // Используем существующий метод из сервиса для обновления токена
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
        
        // Получаем новый токен
        var newToken = await _tokenService.GetNewAccessToken();
        
        if (string.IsNullOrEmpty(newToken))
        {
            throw new UnauthorizedAccessException("Access token is missing after refresh");
        }
        
        // Создаем новый запрос (старый уже отправлен)
        var newRequest = await _CloneRequestAsync(originalRequest);
        newRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        
        // Повторяем запрос с новым токеном
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
        
        // Копируем контент
        if (original.Content != null)
        {
            var ms = new MemoryStream();
            await original.Content.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);
            
            // Копируем заголовки контента
            foreach (var header in original.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
        
        // Копируем заголовки запроса
        foreach (var header in original.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        
        return clone;
    }
}