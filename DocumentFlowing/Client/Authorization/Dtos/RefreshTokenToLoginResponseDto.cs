using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.Dtos;

public class RefreshTokenToLoginResponseDto
{
    [JsonPropertyName("isAllowed")]
    public bool IsAllowed { get; set; }
}