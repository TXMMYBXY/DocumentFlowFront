using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Admin.ViewModels;

public class GetUserViewModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("passwordHash")]
    public string PasswordHash { get; set; }
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    [JsonPropertyName("role:{title}")]
    public string Role { get; set; }
    [JsonPropertyName("department:{title}")]
    public string Department { get; set; }
}