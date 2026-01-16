using DocumentFlowing.Client.Admin.Dtos;

namespace DocumentFlowing.Interfaces.Client;

public interface IAdminClient
{
    /// <summary>
    /// Получение списка всех пользователей
    /// </summary>
    /// <param name="uri">users/get-all</param>
    Task<List<GetUserDto>> GetAllUsersAsync();
    
    /// <summary>
    /// Создание нового пользователя users/add-user
    /// </summary>
    /// <param name="createNewUserDto">DTO с новым пользователем</param>
    Task CreateNewUserAsync(CreateNewUserDto createNewUserDto);
    
    /// <summary>
    /// Смена статуса пользователя users/{userId}/change-status
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    Task<bool> ChangeStatusByIdAsync(int userId);
    
    /// <summary>
    /// Удаление пользователя users/delete-user
    /// </summary>
    /// <param name="selectedUserId">ID пользователя</param>
    Task DeleteUserByIdAsync(int selectedUserId);
    
    /// <summary>
    /// Смена пароля пользователя users/{userId}/reset-password
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="resetPasswordDto">DTO с новым паролем</param>
    Task ChangePasswordByIdAsync(int userId, ResetPasswordDto resetPasswordDto);
    
    /// <summary>
    /// Обновление информации о пользователе users/{userId}/update-user-info
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="updateUserDto">DTO с новой информацией</param>
    Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
}