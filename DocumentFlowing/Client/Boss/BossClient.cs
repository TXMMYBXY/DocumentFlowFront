using DocumentFlowing.Client.Boss.Dtos;
using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Windows.Documents;

namespace DocumentFlowing.Client.Boss;

public class BossClient : GeneralClient, IBossClient
{
    public BossClient(HttpClient httpClient, IOptions<DocumentFlowApi> documentFlowApi) : base(httpClient, documentFlowApi)
    {
    }

    public async Task<List<GetTemplateDto>> GetAllTemplatesAsync(string uri)
    {
        return await GetResponseAsync<List<GetTemplateDto>>(uri);
    }

    public async Task CreateNewTemplateAsync(CreateTemplateDto createTemplateDto)
    {
        await PostResponseAsync<CreateTemplateDto, CreateTemplateDto>(createTemplateDto, "templates/add-template");
    }

    public async Task ChangeStatusByIdAsync(int templateId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteTemplateByIdAsync(int templateId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTemplateAsync(int templateId, UpdateTemplateDto updateTemplateDto)
    {
        throw new NotImplementedException();
    }
}