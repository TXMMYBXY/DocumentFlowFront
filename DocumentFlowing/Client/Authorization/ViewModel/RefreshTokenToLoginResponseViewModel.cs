using DocumentFlowing.Models;
using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModel;

public class RefreshTokenToLoginResponseViewModel
{
    [JsonPropertyName("isAllowed")]
    public bool IsAllowed { get; set; }
    [JsonPropertyName("refreshToken")]
    public RefreshToken RefreshToken { get; set; }
}