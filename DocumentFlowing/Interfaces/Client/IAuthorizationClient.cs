using DocumentFlowing.Client.Authorization.Dtos;

namespace DocumentFlowing.Interfaces.Client;

public interface IAuthorizationClient
{
    /// <summary>
    /// Метод для авторизации по почте и паролю
    /// </summary>
    /// <param name="request">DTO с почтой и паролем</param>
    /// <param name="uri">эндпоинт</param>
    /// <returns>DTO с информацией о токенах</returns>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string uri);
    
    /// <summary>
    /// Метод для авторизации по рефреш-токену
    /// </summary>
    /// <param name="request">DTO с рефреш-токеном</param>
    /// <param name="uri">эндпоинт</param>
    /// <returns>DTO с допуском или не допуском</returns>
    Task<RefreshTokenToLoginResponseDto> RequestForAccessAsync(RefreshTokenToLoginRequestDto request, string uri);
    
    /// <summary>
    /// Метод для получения нового токена доступа по рефреш-токену
    /// </summary>
    /// <param name="requestDto">DTO с рефреш-токеном и ID пользователя</param>
    /// <param name="uri">эндпоинт</param>
    /// <returns>DTO с обновленной информацией о токенах</returns>
    Task<AccessTokenResponseDto> GetNewAccessTokenAsync(AccessTokenRequestDto requestDto, string uri);
    
    /// <summary>
    /// Метод для обновления рефреш-токена по старому
    /// </summary>
    /// <param name="requestDto">DTO с рефреш-токеном и ID пользователя</param>
    /// <param name="uri">эндпоинт</param>
    /// <returns>DTO с обновленной информацией о рефреш-токене</returns>
    Task<RefreshTokenResponseDto> GetNewRefreshTokenAsync(RefreshTokenRequestDto requestDto, string uri);
}