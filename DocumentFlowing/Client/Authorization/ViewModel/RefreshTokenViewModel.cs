using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModel;

public class RefreshTokenViewModel
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}