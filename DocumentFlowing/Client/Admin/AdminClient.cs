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
}