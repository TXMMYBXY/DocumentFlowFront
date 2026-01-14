using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using DocumentFlowing.Models.Admin;
using DocumentFlowing.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Admin;

public class UpdateUserViewModel : BaseViewModel, IDialogService
{
    private readonly UpdateUserModel _updateUserModel;
    private string _errorMessage;
    private bool _isLoading;
    private bool _canCreate;

    private int _userId;
    private string _email;
    private string _fullName;
    private string _department;
    private Role _selectedRole;
    
    public string Email
    {
        get => _email;
        set
        {
            SetField(ref _email, value);
            _ValidateForm();
        }
    }
    
    public string FullName
    {
        get => _fullName;
        set
        {
            SetField(ref _fullName, value);
            _ValidateForm();
        }
    }
        
    public Role SelectedRole
    {
        get => _selectedRole;
        set
        {
            SetField(ref _selectedRole, value);
            _ValidateForm();
        }
    }
        
    public string Department
    {
        get => _department;
        set
        {
            SetField(ref _department, value);
            _ValidateForm();
        }
    }
    
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetField(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetField(ref _isLoading, value);
    }
    
    public bool CanCreate
    {
        get => _canCreate;
        private set => SetField(ref _canCreate, value);
    }
    
    public ObservableCollection<Role> Roles { get; } = new();
    public ObservableCollection<string> DepartmentSuggestions { get; } = new();

    public UpdateUserViewModel(IAdminClient adminClient, UpdateUserDto updateUserDto, int userId)
    {
        _updateUserModel = new UpdateUserModel(adminClient);
        _userId = userId;
        
        _LoadInitialDataAsync(updateUserDto);
        
        SaveCommand = new RelayCommand(async () => await _UpdateUserAsync());
        CancelCommand = new RelayCommand(() => DialogClosed?.Invoke(true));
    }
    
    public event Action<bool>? DialogClosed;
    
    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    
    
    private async Task _UpdateUserAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
                
            var updateUserDto = new UpdateUserDto
            {
                Email = Email,
                FullName = FullName,
                RoleId = SelectedRole.Id,
                Department = Department
            };
                
            var result = await _updateUserModel.UpdateUserAsync(_userId, updateUserDto);
                
            if (result)
            {
                MessageBox.Show("Данные пользователя успешно обновлены", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    
                DialogClosed?.Invoke(true);
            }
            else
            {
                ErrorMessage = "Не удалось обновить данные пользователя";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private async Task _LoadInitialDataAsync(UpdateUserDto updateUserDto)
    {
        try
        {
            IsLoading = true;
            
            await Task.Run(() =>
            {
                var roles = _updateUserModel.GetRoles();
                var departments = _updateUserModel.GetDepartmentSuggestions();
                    
                // Обновляем UI в основном потоке
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Roles.Clear();
                    foreach (var role in roles)
                    {
                        Roles.Add(role);
                    }
                        
                    DepartmentSuggestions.Clear();
                    foreach (var dept in departments)
                    {
                        DepartmentSuggestions.Add(dept);
                    }
                        
                    Email = updateUserDto.Email;
                    FullName = updateUserDto.FullName;
                    Department = updateUserDto.Department;
                    SelectedRole = Roles.FirstOrDefault(role => role.Id == updateUserDto.RoleId);
                });
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка загрузки данных: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private void _ValidateForm()
    {
        ErrorMessage = string.Empty;
            
        if (!string.IsNullOrWhiteSpace(Email) && !_updateUserModel.ValidateEmail(Email))
        {
            ErrorMessage = "Некорректный email адрес";
            CanCreate = false;
            return;
        }
        
        if (string.IsNullOrWhiteSpace(FullName))
        {
            ErrorMessage = "Пустые ФИО";
            CanCreate = false;
            return;
        }
        
        CanCreate = !string.IsNullOrWhiteSpace(Email) &&
                    !string.IsNullOrWhiteSpace(FullName) &&
                    !string.IsNullOrWhiteSpace(Department) &&
                    SelectedRole != null;
    }
}