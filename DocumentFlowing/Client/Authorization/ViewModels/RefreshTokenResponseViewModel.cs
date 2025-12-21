using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModels;

public class RefreshTokenResponseViewModel
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
}