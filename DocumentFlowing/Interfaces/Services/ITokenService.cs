using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Client.Authorization.ViewModels;
using DocumentFlowing.Models;

namespace DocumentFlowing.Interfaces.Client.Services;

/// <summary>
/// Сервис взаимодействует с реестром windows (Сохраняет/возвращает/валидирует) токены из реестра
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Сохраняет токены
    /// </summary>
    void SaveTokens(LoginResponseDto loginResponseDto);
    
    /// <summary>
    /// Сохраняет токен обновления
    /// </summary>
    void SaveRefreshToken(RefreshTokenResponseViewModel refreshTokenResponse);
    
    /// <summary>
    /// Возвращает токен доступа
    /// </summary>
    string ReturnAccessToken();
    
    /// <summary>
    /// Возвращает токен обновления
    /// </summary>
    string ReturnRefreshToken();
    
    /// <summary>
    /// Возвращает информацию о пользователе
    /// </summary>
    UserInfoDto GetUserInfo();
    
    /// <summary>
    /// Проверяет есть ли валидный токен доступа
    /// </summary>
    bool IsAccessTokenValid();
    
    /// <summary>
    /// Проверяет есть ли валидный токен обновления
    /// </summary>
    bool IsRefreshTokenValid();
    
    /// <summary>
    /// Очищает реестр от токенов
    /// </summary>
    void ClearTokens();
    
    /// <summary>
    /// Получить новый токен доступа
    /// </summary>
    Task<string> GetNewAccessTokenAsync();
    
    /// <summary>
    /// Получить новый токен обновления
    /// </summary>
    Task GetNewRefreshTokenAsync();

    bool IsRefreshTokenExpires();
}