using System.Text.Json.Serialization;

namespace DocumentFlowing.Models;

public class Department
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}