using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Boss.Dtos;

public class GetTemplateDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("path")]
    public string Path { get; set; }
    [JsonPropertyName("createdBy")]
    [ForeignKey(nameof(CreatedBy))]
    public int CreatedBy { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}