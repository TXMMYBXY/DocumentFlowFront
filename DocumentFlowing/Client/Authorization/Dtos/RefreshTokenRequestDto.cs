using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.Dtos;

public class RefreshTokenRequestDto
{
    [JsonPropertyName("userId")]
    public int? UserId { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; }
}