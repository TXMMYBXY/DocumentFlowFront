using System.Text.Json.Serialization;

namespace DocumentFlowing.Models;
public class LoginResponse
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
    public RefreshToken RefreshToken { get; set; }
}
