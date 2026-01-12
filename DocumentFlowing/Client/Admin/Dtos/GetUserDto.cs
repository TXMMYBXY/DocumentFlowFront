using DocumentFlowing.Models;
using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Admin.Dtos;

public class GetUserDto
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
    [JsonPropertyName("department")]
    public string Department { get; set; }
    [JsonPropertyName("role")]
    public Role RoleEntity { get; set; }
    public string Role => RoleEntity.Title;
}