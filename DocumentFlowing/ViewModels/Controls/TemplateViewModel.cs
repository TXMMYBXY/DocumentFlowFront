using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models.Boss;
using DocumentFlowing.ViewModels.Base;
using DocumentFlowing.ViewModels.Controls.Items;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Controls;

public class TemplateViewModel : BaseViewModel
{
    private readonly TemplateModel _templateModel;
    
    private ObservableCollection<TemplateItemViewModel> _templates = new();
    private ICollectionView _templatesView;
    
    private bool _isLoading;
    private string _errorMessage;
    private TemplateItemViewModel _selectedTemplate;
    private string _searchText;
    private int _countFounded;

    public ObservableCollection<TemplateItemViewModel> Templates
    {
        get => _templates;
        set
        {
            SetField(ref _templates, value);
            _CreateTemplateView();
        }
    }
    
    public ICollectionView TemplatesView
    {
        get => _templatesView;
        private set => SetField(ref _templatesView, value);
    }
    
    public bool IsLoading
    {
        get => _isLoading;
        set => SetField(ref _isLoading, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetField(ref _errorMessage, value);
    }

    public TemplateItemViewModel SelectedTemplate
    {
        get => _selectedTemplate;
        set => SetField(ref _selectedTemplate, value);
    }
    
    public int CountFounded
    {
        get => _countFounded;
        private set => SetField(ref _countFounded, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetField(ref _searchText, value))
            {
                _ApplyFilter();
            }
        }
    }
    
    public ICommand AddTemplateCommand { get; set; }
    public ICommand RefreshCommand { get; set; }
    public ICommand ChangeTemplateStatusCommand { get; set; }
    
    public TemplateViewModel(IBossClient bossClient, INavigationService navigationService)
    {
        _templateModel = new TemplateModel(bossClient, navigationService);
        
        AddTemplateCommand = new RelayCommand(() => throw new NotImplementedException());
        RefreshCommand = new RelayCommand(async () => await _LoadTemplatesAsync());
        ChangeTemplateStatusCommand =
            new RelayCommand(async () => await _ChangeUserStatusAsync());
        
        _ = _LoadTemplatesAsync();
    }

    private async Task _LoadTemplatesAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
                
            var templatesList = await _templateModel.GetAllTemplatesAsync();
                
            Templates.Clear();
            foreach (var user in templatesList)
            {
                Templates.Add(new TemplateItemViewModel(user));
            }
            
            _CreateTemplateView();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка загрузки шаблонов: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private void _ApplyFilter()
    {
        TemplatesView?.Refresh();
        
        if (TemplatesView != null && !string.IsNullOrWhiteSpace(SearchText))
        {
            CountFounded = TemplatesView.OfType<UserItemViewModel>().Count();
        }
    }

    private void _CreateTemplateView()
    {
        TemplatesView = CollectionViewSource.GetDefaultView(Templates);
        TemplatesView.Filter = _TemplateFilter;
        TemplatesView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
    }

    private bool _TemplateFilter(object item)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
            return true;

        if (!(item is TemplateItemViewModel template))
            return false;

        var searchLower = SearchText.ToLower();
        
        return template.Title?.ToLower().Contains(searchLower) == true ||
               template.CreatedBy.ToString().Contains(searchLower) == true ||
               template.Path.ToLower().Contains(searchLower) == true ||
               template.CreatedAt.Date.ToString().Contains(searchLower) == true;
    }
    
    private async Task _ChangeUserStatusAsync()
    {
        if (SelectedTemplate == null) return;
        
        try
        {
            IsLoading = true;
            
            var newStatus = await _templateModel.ChangeStatusByIdAsync(SelectedTemplate.Id);
            
            SelectedTemplate.IsActive = newStatus;
            
            _ApplyFilter();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка изменения статуса: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}