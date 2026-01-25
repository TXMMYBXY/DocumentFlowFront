using DocumentFlowing.Client.Boss.Dtos;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Views.Boss;

namespace DocumentFlowing.Models.Boss;

public class StatementTemplateModel : ITemplateEndpoint
{
    public static string BaseEndpoint { get; } = "statement-template";
    
    private readonly IBossClient _bossClient;
    private readonly INavigationService _navigationService;
    
    public StatementTemplateModel(IBossClient bossClient, INavigationService navigationService)
    {
        _bossClient = bossClient;
        _navigationService = navigationService;
    }

    public async Task<List<GetTemplateDto>> GetAllTemplatesAsync()
    {
        return await _bossClient.GetAllTemplatesAsync<ContractTemplateModel>();
    }

    public async Task<bool> ChangeStatusByIdAsync(int templateId)
    {
        return await _bossClient.ChangeStatusByIdAsync<ContractTemplateModel>(templateId);
    }

    public void OpenModalWindowCreateTemplate()
    {
        _navigationService.ShowDialog<CreateTemplateView>();
    }

    public async Task DeleteTemplateByIdAsync(int templateId)
    {
        await _bossClient.DeleteTemplateByIdAsync<ContractTemplateModel>(templateId);
    }
}