using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentFlowing.Models;
public class LoginResponse
{
    [JsonPropertyName("userInfo")]
    public UserInfoDto UserInfo { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; } = "Bearer";
}
