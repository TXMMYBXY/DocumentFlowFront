using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Models;
using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.ViewModels;

public class RefreshTokenToLoginResponseViewModel
{
    [JsonPropertyName("isAllowed")]
    public bool IsAllowed { get; set; }
    [JsonPropertyName("refreshToken")]
    public RefreshTokenDto RefreshTokenDto { get; set; }
}