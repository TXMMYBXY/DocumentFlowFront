using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentFlowing.Models;
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
