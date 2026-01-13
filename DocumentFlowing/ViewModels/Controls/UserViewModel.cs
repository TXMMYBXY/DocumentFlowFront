using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using DocumentFlowing.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Controls;

public class UserViewModel : BaseViewModel
{
    private readonly UserModel _userModel;
    
    private ObservableCollection<UserItemViewModel> _users = new();
    
    private bool _isLoading;
    private string _errorMessage;
    private UserItemViewModel _selectedUser;
        
    public ObservableCollection<UserItemViewModel> Users
    {
        get => _users;
        set => SetField(ref _users, value);
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

    public UserItemViewModel SelectedUser
    {
        get => _selectedUser;
        set => SetField(ref _selectedUser, value);
    }
        
    public ICommand AddUserCommand { get; private set; }
    public ICommand RefreshCommand { get; private set; }
    public ICommand EditUserCommand { get; private set; }
    public ICommand ChangePasswordCommand { get; private set; }
    public ICommand ChangeUserStatusCommand { get; private set; }
    public ICommand DeleteUserCommand { get; private set; }

    public UserViewModel(IAdminClient adminClient, INavigationService navigationService)
    {
        _userModel = new UserModel(adminClient, navigationService);
  
        // Инициализация команд
        _InitializeCommands();

        // Запускаем загрузку, но не блокируем конструктор
        _ = _InitializeAsync();
    }

    private void _InitializeCommands()
    {
        AddUserCommand = new RelayCommand(() =>  _userModel.OpenModalWindowCreateUser());
        RefreshCommand = new RelayCommand(async () => await _LoadUsersAsync());
        EditUserCommand = new RelayCommand(() => throw new NotImplementedException());
        ChangePasswordCommand = new RelayCommand(() => _ChangePasswordAsync());
        ChangeUserStatusCommand = new RelayCommand(async () => await _ChangeUserStatusAsync());
        DeleteUserCommand = new RelayCommand(async () => await _DeleteUserAsync());
    }

    private async Task _InitializeAsync()
    {
        await _LoadUsersAsync();
    }

    private async Task _LoadUsersAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
                
            var userList = await _userModel.GetAllUsersAsync();
                
            Users.Clear();
            foreach (var user in userList)
            {
                Users.Add(new UserItemViewModel(user));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка загрузки пользователей: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task _ChangeUserStatusAsync()
    {
        try
        {
            IsLoading = true;
            
            var newStatus = await _userModel.ChangeStatusByIdAsync(SelectedUser.Id);
            
            SelectedUser.IsActive = newStatus;
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

    private async Task _DeleteUserAsync()
    {
        var result = MessageBox.Show($"Вы собираетесь удалить пользователя {SelectedUser.FullName}. Продолжить?", 
            "Удаление пользователя",
            MessageBoxButton.YesNo, 
            MessageBoxImage.Warning);
        
        if (result == MessageBoxResult.No) return;
        try
        {
            IsLoading = true;

            await _userModel.DeleteUserByIdAsync(SelectedUser.Id);
            
            Users.Remove(SelectedUser);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка удаления: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void _ChangePasswordAsync()
    {
        _userModel.OpenModalWindowChangePassword(SelectedUser.Id);
    }
}