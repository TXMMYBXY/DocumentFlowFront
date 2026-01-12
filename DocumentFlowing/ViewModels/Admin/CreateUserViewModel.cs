using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Common;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
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
        
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetField(ref _email, value);
                ValidateForm();
            }
        }
        
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetField(ref _password, value);
                ValidateForm();
            }
        }
        
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetField(ref _confirmPassword, value);
                ValidateForm();
            }
        }
        
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                SetField(ref _fullName, value);
                ValidateForm();
            }
        }
        
        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                SetField(ref _selectedRole, value);
                ValidateForm();
            }
        }
        
        private string _department;
        public string Department
        {
            get => _department;
            set
            {
                SetField(ref _department, value);
                ValidateForm();
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
        public ObservableCollection<Role> Roles { get; } = new ObservableCollection<Role>();
        public ObservableCollection<string> DepartmentSuggestions { get; } = new ObservableCollection<string>();
        
        // Флаг возможности создания
        private bool _canCreate;
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
            CreateCommand = new RelayCommand(async () => await CreateUserAsync(), 
                () => CanCreate && !IsLoading);
            CancelCommand = new RelayCommand(() => DialogClosed?.Invoke(true));
            
            // Загружаем данные
            LoadInitialDataAsync();
        }
        
        private async Task LoadInitialDataAsync()
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
                        
                        // Устанавливаем значения по умолчанию
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
        private void ValidateForm()
        {
            ErrorMessage = string.Empty;
            
            // Проверка email
            if (!string.IsNullOrWhiteSpace(Email) && !_createUserModel.ValidateEmail(Email))
            {
                ErrorMessage = "Некорректный email адрес";
                CanCreate = false;
                return;
            }
            
            // Проверка пароля
            if (!string.IsNullOrWhiteSpace(Password) && Password.Length < 3)
            {
                ErrorMessage = "Пароль должен содержать не менее 4 символов";
                CanCreate = false;
                return;
            }
            
            // Проверка на совпадение паролей
            ShowPasswordMismatchError = !string.IsNullOrEmpty(ConfirmPassword) && Password != ConfirmPassword;
            
            // Проверка обязательных полей
            CanCreate = !ShowPasswordMismatchError &&
                       !string.IsNullOrWhiteSpace(Email) &&
                       !string.IsNullOrWhiteSpace(Password) &&
                       !string.IsNullOrWhiteSpace(FullName) &&
                       !string.IsNullOrWhiteSpace(Department) &&
                       SelectedRole != null;
        }
        
        private async Task CreateUserAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                
                // Создаем DTO
                var createUserDto = new CreateNewUserDto
                {
                    Email = Email,
                    Password = Password,
                    FullName = FullName,
                    RoleId = SelectedRole?.Id ?? 0,
                    Department = Department
                };
                
                // Вызываем Model
                var result = await _createUserModel.CreateUserAsync(createUserDto);
                
                if (result)
                {
                    // Успешное создание
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