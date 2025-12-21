using System.Text.Json.Serialization;

namespace DocumentFlowing.Client.Models;

public class ErrorResponse
{
    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;
}