using DocumentFlowing.Client.Boss.Dtos;
using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace DocumentFlowing.Client.Boss;

public class BossClient : GeneralClient, IBossClient
{
    public BossClient(HttpClient httpClient, IOptions<DocumentFlowApi> documentFlowApi) : base(httpClient, documentFlowApi)
    {
    }

    public async Task<List<GetTemplateDto>> GetAllTemplatesAsync()
    {
        return await GetResponseAsync<List<GetTemplateDto>>("contract-templates/get-all");
    }

    public async Task CreateNewTemplateAsync(CreateTemplateDto createTemplateDto)
    {
        await PostResponseAsync<CreateTemplateDto, CreateTemplateDto>(createTemplateDto, "contract-templates/add-template");
    }

    public async Task<bool> ChangeStatusByIdAsync(int templateId)
    {
        return await PatchResponseAsync<object, bool>(null, $"contract-templates/{templateId}/change-template-status");
    }

    public async Task DeleteTemplateByIdAsync(int templateId)
    {
        await DeleteResponseAsync<DeleteTemplateDto, object>(new DeleteTemplateDto {TemplateId = templateId}, 
            "contract-templates/delete-template");
    }

    public async Task UpdateTemplateAsync(int templateId, UpdateTemplateDto updateTemplateDto)
    {
        await PatchResponseAsync<UpdateTemplateDto, object>(updateTemplateDto,
            $"contract-templates/{templateId}/update-template");
    }
}