using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;

namespace DocumentFlowing.Models.Admin;

public class UpdateUserModel
{
    private readonly IAdminClient _adminClient;

    public UpdateUserModel(IAdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
    {
        await _adminClient.UpdateUserAsync(userId, updateUserDto);
        
        return true;
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