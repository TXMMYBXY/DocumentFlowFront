using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models.Boss;
using DocumentFlowing.ViewModels.Base;

namespace DocumentFlowing.ViewModels.Controls;

public class TemplateViewModel : BaseViewModel
{
    private readonly TemplateModel _templateModel;

    public TemplateViewModel(IBossClient bossClient, INavigationService navigationService)
    {
        _templateModel = new TemplateModel(bossClient, navigationService);
    }
}