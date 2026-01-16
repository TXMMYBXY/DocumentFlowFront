using DocumentFlowing.Client.Boss.Dtos;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;

namespace DocumentFlowing.Models.Boss;

public class TemplateModel
{
    private readonly IBossClient _bossClient;
    private readonly INavigationService _navigationService;
    
    public TemplateModel(IBossClient bossClient, INavigationService navigationService)
    {
        _bossClient = bossClient;
        _navigationService = navigationService;
    }

    public async Task<List<GetTemplateDto>> GetAllTemplatesAsync()
    {
        return await _bossClient.GetAllTemplatesAsync();
    }

    public async Task<bool> ChangeStatusByIdAsync(int templateId)
    {
        return await _bossClient.ChangeStatusByIdAsync(templateId);
    }
}