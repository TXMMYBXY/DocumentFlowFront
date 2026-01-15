using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.Dtos;

public class RefreshTokenResponseDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
}