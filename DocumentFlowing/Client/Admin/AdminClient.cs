using DocumentFlowing.Client.Admin.ViewModels;
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

    public async Task<List<GetUserViewModel>> GetAllUsersAsync(string uri)
    {
        return await GetResponseAsync<List<GetUserViewModel>>(uri);
    }
}