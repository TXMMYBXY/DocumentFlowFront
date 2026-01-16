using DocumentFlowing.Client.Boss.Dtos;

namespace DocumentFlowing.Interfaces.Client;

public interface IBossClient
{
    Task<List<GetTemplateDto>> GetAllTemplatesAsync();
    Task CreateNewTemplateAsync(CreateTemplateDto createTemplateDto);
    Task ChangeStatusByIdAsync(int templateId);
    Task DeleteTemplateByIdAsync(int templateId);
    Task UpdateTemplateAsync(int templateId, UpdateTemplateDto updateTemplateDto);
}