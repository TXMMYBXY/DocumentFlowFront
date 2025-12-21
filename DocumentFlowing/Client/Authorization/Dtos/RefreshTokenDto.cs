using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.Dtos;
public class RefreshTokenDto
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
}
