using DocumentFlowing.Client.Boss.Dtos;
using DocumentFlowing.Interfaces.Client.Services;

namespace DocumentFlowing.Interfaces.Client;

public interface IBossClient
{
    Task<List<GetTemplateDto>> GetAllTemplatesAsync<T>() where T : ITemplateEndpoint;
    Task CreateNewTemplateAsync<T>(CreateTemplateDto createTemplateDto) where T : ITemplateEndpoint;
    Task<bool> ChangeStatusByIdAsync<T>(int templateId) where T : ITemplateEndpoint;
    Task DeleteTemplateByIdAsync<T>(int templateId) where T : ITemplateEndpoint;
    Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto updateTemplateDto) where T : ITemplateEndpoint;
}