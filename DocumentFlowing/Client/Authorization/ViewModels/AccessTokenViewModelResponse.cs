using DocumentFlowing.Client.Authorization.Dtos;
using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModels;

public class AccessTokenViewModelResponse
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
    public RefreshTokenDto RefreshToken { get; set; }
}