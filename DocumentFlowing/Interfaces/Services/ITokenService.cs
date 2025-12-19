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
    void SaveTokens(LoginResponse loginResponse);
    
    /// <summary>
    /// Сохраняет токен обновления
    /// </summary>
    void SaveRefreshToken(RefreshTokenResponseViewModel refreshTokenResponse);
    
    /// <summary>
    /// Возвращает токен доступа
    /// </summary>
    string GetAccessToken();
    
    /// <summary>
    /// Возвращает токен обновления
    /// </summary>
    string GetRefreshToken();
    
    /// <summary>
    /// Возвращает информацию о пользователе
    /// </summary>
    UserInfoDto GetUserInfo();
    
    /// <summary>
    /// Проверяет есть ли валидный токен доступа
    /// </summary>
    bool HasValidAccessToken();
    /// <summary>
    /// Проверяет есть ли валидный токен обновления
    /// </summary>
    bool IsRefreshTokenValid();
    
    //Пока не используются
    int? GetRefreshTokenId();
    int? GetUserId();
}