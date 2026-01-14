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

namespace DocumentFlowing.ViewModels.Admin
{
    public class CreateUserViewModel : BaseViewModel, IDialogService
    {
        private readonly CreateUserModel _createUserModel;
        private bool _isLoading;
        private string _errorMessage;
        private bool _canCreate;
        
        private string _email;
        private string _password;
        private string _confirmPassword;
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
        
        public string Password
        {
            get => _password;
            set
            {
                SetField(ref _password, value);
                _ValidateForm();
            }
        }
        
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetField(ref _confirmPassword, value);
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
        
        // Коллекции для ComboBox
        public ObservableCollection<Role> Roles { get; } = new ();
        public ObservableCollection<string> DepartmentSuggestions { get; } = new ();
        
        
        public bool CanCreate
        {
            get => _canCreate;
            private set => SetField(ref _canCreate, value);
        }
        
        private bool _showPasswordMismatchError;
        public bool ShowPasswordMismatchError
        {
            get => _showPasswordMismatchError;
            set => SetField(ref _showPasswordMismatchError, value);
        }
        
        // Команды
        public ICommand CreateCommand { get; }
        public ICommand CancelCommand { get; }
        
        // Событие для закрытия окна
        public event Action<bool> DialogClosed;
        
        public CreateUserViewModel(IAdminClient adminClient)
        {
            _createUserModel = new CreateUserModel(adminClient);
            
            // Инициализация команд
            CreateCommand = new RelayCommand(async () => await _CreateUserAsync(), 
                () => CanCreate && !IsLoading);
            CancelCommand = new RelayCommand(() => DialogClosed?.Invoke(true));
            
            // Загружаем данные
            _LoadInitialDataAsync();
        }
        
        private async Task _LoadInitialDataAsync()
        {
            try
            {
                IsLoading = true;
                
                // Загружаем роли и подсказки для отделов
                await Task.Run(() =>
                {
                    var roles = _createUserModel.GetRoles();
                    var departments = _createUserModel.GetDepartmentSuggestions();
                    
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
                        
                        if (Roles.Any())
                            SelectedRole = Roles.First();
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
        
        /// <summary>
        /// Валидация формы
        /// </summary>
        private void _ValidateForm()
        {
            ErrorMessage = string.Empty;
            
            if (!string.IsNullOrWhiteSpace(Email) && !_createUserModel.ValidateEmail(Email))
            {
                ErrorMessage = "Некорректный email адрес";
                CanCreate = false;
                return;
            }
            
            if (!string.IsNullOrWhiteSpace(Password) && Password.Length < 3)
            {
                ErrorMessage = "Пароль должен содержать не менее 4 символов";
                CanCreate = false;
                return;
            }
            
            ShowPasswordMismatchError = !string.IsNullOrEmpty(ConfirmPassword) && Password != ConfirmPassword;
            
            CanCreate = !ShowPasswordMismatchError &&
                       !string.IsNullOrWhiteSpace(Email) &&
                       !string.IsNullOrWhiteSpace(Password) &&
                       !string.IsNullOrWhiteSpace(FullName) &&
                       !string.IsNullOrWhiteSpace(Department) &&
                       SelectedRole != null;
        }
        
        private async Task _CreateUserAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                
                var createUserDto = new CreateNewUserDto
                {
                    Email = Email,
                    Password = Password,
                    FullName = FullName,
                    RoleId = SelectedRole?.Id ?? 0,
                    Department = Department
                };
                
                var result = await _createUserModel.CreateUserAsync(createUserDto);
                
                if (result)
                {
                    MessageBox.Show("Пользователь успешно создан", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    DialogClosed?.Invoke(true);
                }
                else
                {
                    ErrorMessage = "Не удалось создать пользователя";
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
    }
}