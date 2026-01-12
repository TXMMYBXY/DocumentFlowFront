using System.Text.Json.Serialization;

namespace DocumentFlowing.Models;

public class Role
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}