using DocumentFlowing.Client.Admin.Dtos;
using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace DocumentFlowing.Client.Admin;

public class AdminClient : GeneralClient, IAdminClient
{
    
    public AdminClient(HttpClient httpClient, IOptions<DocumentFlowApi> documentFlowApi) : base(httpClient, documentFlowApi)
    {
    }

    public async Task<List<GetUserDto>> GetAllUsersAsync(string uri)
    {
        return await GetResponseAsync<List<GetUserDto>>(uri);
    }

    public async Task<object> CreateNewUserAsync(CreateNewUserDto createNewUserDto)
    {
        return await PostResponseAsync<CreateNewUserDto, CreateNewUserDto>(createNewUserDto, "users/add-user");
    }

    public async Task<bool> ChangeStatusByIdAsync(int userId)
    {
        return await UpdateResponseAsync<object, bool>(null, $"users/{userId}/change-status");
    }

    public async Task DeleteUserByIdAsync(int selectedUserId)
    {
        await DeleteResponseAsync<DeleteUserDto, object>(new DeleteUserDto{UserId =  selectedUserId}, "users/delete-user");
    }

    public async Task ChangePasswordByIdAsync(int userId, ResetPasswordDto resetPasswordDto)
    {
        await UpdateResponseAsync<ResetPasswordDto, object>(resetPasswordDto, $"users/{userId}/reset-password");
    }

    public async Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
    {
        await UpdateResponseAsync<UpdateUserDto, object>(updateUserDto, $"users/{userId}/update-user-info");
    }
}