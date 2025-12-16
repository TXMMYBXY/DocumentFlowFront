using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModel;

public class RefreshTokenResponseViewModel
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
}