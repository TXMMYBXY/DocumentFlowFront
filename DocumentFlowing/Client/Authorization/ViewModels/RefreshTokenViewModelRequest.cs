using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModels;

public class RefreshTokenViewModelRequest
{
    [JsonPropertyName("userId")]
    public int? UserId { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; }
}