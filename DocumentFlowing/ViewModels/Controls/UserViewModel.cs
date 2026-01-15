using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using DocumentFlowing.Models.Admin;
using DocumentFlowing.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Controls;

public class UserViewModel : BaseViewModel
{
    private readonly UserModel _userModel;
    
    private ObservableCollection<UserItemViewModel> _users = new();
    private ICollectionView _usersView;
    
    private bool _isLoading;
    private string _errorMessage;
    private UserItemViewModel _selectedUser;
    private string _searchText;
    private int _countFounded;
    
    public ObservableCollection<UserItemViewModel> Users
    {
        get => _users;
        set
        {
            SetField(ref _users, value);
            _CreateUsersView();
        }
    }
    
    public ICollectionView UsersView
    {
        get => _usersView;
        private set => SetField(ref _usersView, value);
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

    public ICommand AddUserCommand { get; private set; }
    public ICommand RefreshCommand { get; private set; }
    public ICommand EditUserCommand { get; private set; }
    public ICommand ChangePasswordCommand { get; private set; }
    public ICommand ChangeUserStatusCommand { get; private set; }
    public ICommand DeleteUserCommand { get; private set; }
    public ICommand ClearSearchCommand { get; private set; }

    public UserViewModel(IAdminClient adminClient, INavigationService navigationService)
    {
        _userModel = new UserModel(adminClient, navigationService);
  
        _InitializeCommands();

        _ = _InitializeAsync();
    }

    private void _InitializeCommands()
    {
        AddUserCommand = new RelayCommand(() => _userModel.OpenModalWindowCreateUser());
        RefreshCommand = new RelayCommand(async () => await _LoadUsersAsync());
        EditUserCommand = new RelayCommand(() => _userModel.OpenModalWindowUpdateUser(
            new UpdateUserDto
            {
                Email = SelectedUser.Email,
                FullName = SelectedUser.FullName,
                RoleId = SelectedUser.RoleEntity.Id,
                Department = SelectedUser.Department
            }, SelectedUser.Id));
        ChangePasswordCommand = new RelayCommand(() => _userModel.OpenModalWindowChangePassword(SelectedUser?.Id ?? 0));
        ChangeUserStatusCommand = new RelayCommand(async () => await _ChangeUserStatusAsync(), 
            () => SelectedUser != null);
        DeleteUserCommand = new RelayCommand(async () => await _DeleteUserAsync(), 
            () => SelectedUser != null);
        ClearSearchCommand = new RelayCommand(() => SearchText = string.Empty);
    }

    private async Task _InitializeAsync()
    {
        await _LoadUsersAsync();
    }

    private void _CreateUsersView()
    {
        UsersView = CollectionViewSource.GetDefaultView(Users);
        UsersView.Filter = _UserFilter;
        UsersView.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));
    }

    private bool _UserFilter(object item)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
            return true;

        if (!(item is UserItemViewModel user))
            return false;

        var searchLower = SearchText.ToLower();
        
        return user.Email?.ToLower().Contains(searchLower) == true ||
               user.FullName?.ToLower().Contains(searchLower) == true ||
               user.Role?.ToLower().Contains(searchLower) == true ||
               user.Department?.ToLower().Contains(searchLower) == true;
    }

    private void _ApplyFilter()
    {
        UsersView?.Refresh();
        
        if (UsersView != null && !string.IsNullOrWhiteSpace(SearchText))
        {
            CountFounded = UsersView.OfType<UserItemViewModel>().Count();
        }
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
            
            _CreateUsersView();
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
        if (SelectedUser == null) return;
        
        try
        {
            IsLoading = true;
            
            var newStatus = await _userModel.ChangeStatusByIdAsync(SelectedUser.Id);
            
            SelectedUser.IsActive = newStatus;
            
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

    private async Task _DeleteUserAsync()
    {
        if (SelectedUser == null) return;
        
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
}