using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModels;

public class AccessTokenViewModelRequest
{
    [JsonPropertyName("userId")]
    public int? UserId { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}