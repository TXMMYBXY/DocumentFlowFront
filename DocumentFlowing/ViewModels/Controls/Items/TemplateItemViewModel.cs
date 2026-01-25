using DocumentFlowing.Client.Boss.Dtos;
using DocumentFlowing.ViewModels.Base;

namespace DocumentFlowing.ViewModels.Controls.Items;

public class TemplateItemViewModel : BaseViewModel
{
    private readonly GetTemplateDto _templateDto;
    
    public bool IsActive
    {
        get => _templateDto.IsActive;
        set
        {
            _templateDto.IsActive = value;
            OnPropertyChanged(nameof(IsActive));
        }
    }
    
    public int Id => _templateDto.Id;
    public string Title => _templateDto.Title;
    public string FilePath => _templateDto.Path;
    public int Owner => _templateDto.CreatedBy;
    public DateTime CreatedAt => _templateDto.CreatedAt;
    
    public TemplateItemViewModel(GetTemplateDto templateDto)
    {
        _templateDto = templateDto;
    }
}