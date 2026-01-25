using DocumentFlowing.Client.Boss.Dtos;
using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Models.Boss;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace DocumentFlowing.Client.Boss;

public class BossClient : GeneralClient, IBossClient
{
    public BossClient(HttpClient httpClient, IOptions<DocumentFlowApi> documentFlowApi) : base(httpClient, documentFlowApi)
    {
    }

    public async Task<List<GetTemplateDto>> GetAllTemplatesAsync<T>() where T : ITemplateEndpoint
    {
        return await GetResponseAsync<List<GetTemplateDto>>($"{T.BaseEndpoint}/get-all");
    }

    public async Task CreateNewTemplateAsync<T>(CreateTemplateDto createTemplateDto) where T : ITemplateEndpoint
    {
        await PostResponseAsync<CreateTemplateDto, CreateTemplateDto>(createTemplateDto, 
            $"{T.BaseEndpoint}/add-template");
    }

    public async Task<bool> ChangeStatusByIdAsync<T>(int templateId) where T : ITemplateEndpoint
    {
        return await PatchResponseAsync<object, bool>(null, 
            $"{T.BaseEndpoint}/{templateId}/change-template-status");
    }

    public async Task DeleteTemplateByIdAsync<T>(int templateId) where T : ITemplateEndpoint
    {
        await DeleteResponseAsync<DeleteTemplateDto, object>(new DeleteTemplateDto {TemplateId = templateId}, 
            $"{T.BaseEndpoint}/delete-template");
    }

    public async Task UpdateTemplateAsync<T>(int templateId, UpdateTemplateDto updateTemplateDto) where T : ITemplateEndpoint
    {
        await PatchResponseAsync<UpdateTemplateDto, object>(updateTemplateDto,
            $"{T.BaseEndpoint}/{templateId}/update-template");
    }
}