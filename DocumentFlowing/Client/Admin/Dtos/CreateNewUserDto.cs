using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Admin.Dtos;

public class CreateNewUserDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("department")]
    public string Department { get; set; }
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
}