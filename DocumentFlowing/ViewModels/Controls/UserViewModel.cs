using DocumentFlowing.Client.Admin.ViewModels;
using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Models;
using DocumentFlowing.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DocumentFlowing.ViewModels.Controls
{
    public class UserViewModel : BaseViewModel
    {
        private ObservableCollection<GetUserViewModel> _users = new();
        private readonly UserModel _userModel;
        private bool _isLoading;
        private string _errorMessage;
        
        public ObservableCollection<GetUserViewModel> Users
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
        
        public ICommand AddUserCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ActivateDeactivateUserCommand { get; }
        
        public UserViewModel(IAdminClient adminClient)
        {
            _userModel = new UserModel(adminClient);
            
            // Инициализация команд
            RefreshCommand = new RelayCommand(async () => await _LoadUsersAsync());
            
            // Запускаем загрузку, но не блокируем конструктор
            _ = _InitializeAsync();
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
                    Users.Add(user);
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
    }
}