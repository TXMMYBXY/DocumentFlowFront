using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModels;

public class RefreshTokenViewModel
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}