namespace DocumentFlowing.Client.Boss.Dtos;

public class CreateTemplateDto
{
    public string Title { get; set; }
    public string Path { get; set; }
    public bool IsActive { get; set; }
}