using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.Dtos;
public class LoginResponseDto
{
    [JsonPropertyName("userInfo")]
    public UserInfoDto UserInfo { get; set; }
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; } = "Bearer";
    [JsonPropertyName("refreshToken")]
    public RefreshTokenDto RefreshTokenDto { get; set; }
}
