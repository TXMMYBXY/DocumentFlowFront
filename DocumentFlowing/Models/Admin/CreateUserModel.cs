using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;

namespace DocumentFlowing.Models.Admin;

public class CreateUserModel
{
    private readonly IAdminClient _adminClient;
    
    public CreateUserModel(IAdminClient adminClient)
    {
        _adminClient = adminClient;
    }
    
    public async Task<bool> CreateUserAsync(CreateNewUserDto userDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userDto.Email))
                throw new ArgumentException("Email обязателен");
                    
            if (string.IsNullOrWhiteSpace(userDto.Password) || userDto.Password.Length < 4)
                throw new ArgumentException("Пароль должен быть не менее 4 символов");
                
            if (string.IsNullOrWhiteSpace(userDto.FullName))
                throw new ArgumentException("ФИО обязательно");
                
            await _adminClient.CreateNewUserAsync(userDto);
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка создания пользователя: {ex.Message}");
            throw;
        }
    }
    
    public bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
                
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
    public List<Role> GetRoles()
    {
        // TODO: Заменить на вызов API, когда будет эндпоинт
        return new List<Role>
        {
            new Role { Id = 1, Title = "Администратор" },
            new Role { Id = 2, Title = "Руководитель" },
            new Role { Id = 3, Title = "Закупщик" },
            new Role { Id = 4, Title = "Пользователь" }
        };
    }
    
    public List<string> GetDepartmentSuggestions()
    {
        return new List<string>
        {
            "IT отдел",
            "Бухгалтерия",
            "Отдел продаж",
            "Отдел кадров",
            "Отдел логистики",
            "Отдел маркетинга"
        };
    }
}