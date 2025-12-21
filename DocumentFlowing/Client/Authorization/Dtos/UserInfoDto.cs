using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Authorization.Dtos;
public class UserInfoDto
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
    [JsonPropertyName("departmentId")]
    public int DepartmentId { get; set; }
}
