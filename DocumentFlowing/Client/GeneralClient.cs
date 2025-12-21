using DocumentFlowing.Client.Models;
using DocumentFlowing.Interfaces.Client;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace DocumentFlowing.Client;
public class GeneralClient : IGeneralClient
{
    private readonly HttpClient _httpClient;
    private readonly DocumentFlowApi _documentFlowApi;

    public GeneralClient(HttpClient httpClient, IOptions<DocumentFlowApi> documentFlowApi)
    {
        _httpClient = httpClient;
        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        _documentFlowApi = documentFlowApi.Value;
    }

    public async Task<TResponse?> UpdateResponseAsync<TRequest, TResponse>(TRequest request, string uri)
    {
        var requestJson = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync(_documentFlowApi.Domain + uri, requestContent);

        await _IsSuccessStatusCode(response);

        var responseJson = await response.Content.ReadAsStringAsync();

        return _ConvertResponse<TResponse>(responseJson);
    }

    public async Task<TResponse?> PostResponseAsync<TRequest, TResponse>(TRequest request, string uri)
    {
        var requestJson = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_documentFlowApi.Domain + uri, requestContent);
        await _IsSuccessStatusCode(response);

        var responseJson = await response.Content.ReadAsStringAsync();

        return _ConvertResponse<TResponse>(responseJson);
    }

    public async Task<TResponse?> DeleteResponseAsync<TRequest, TResponse>(TRequest request, string uri)
    {
        var requestJson = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var requestDelete = new HttpRequestMessage()
        {
            Method = HttpMethod.Delete,
            Content = requestContent,
            RequestUri = new Uri(_documentFlowApi.Domain + uri)
        };
        requestDelete.Headers.Add("accept", "text/plain");
        var response = await _httpClient.SendAsync(requestDelete);

        await _IsSuccessStatusCode(response);

        var responseJson = await response.Content.ReadAsStringAsync();

        return _ConvertResponse<TResponse>(responseJson);
    }

    public async Task<TResponse?> GetResponseAsync<TResponse>(string uri)
    {
        var response = await _httpClient.GetAsync(_documentFlowApi.Domain + uri);

        await _IsSuccessStatusCode(response);

        var responseJson = await response.Content.ReadAsStringAsync();

        return _ConvertResponse<TResponse>(responseJson);
    }

    private static async Task _IsSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorResponse>(errorContent);

            // Передаём статус-код в конструктор HttpRequestException
            throw new HttpRequestException(
                $"Request failed with status {response.StatusCode}, message: {result.Message}",
                null,
                response.StatusCode
            );
        }
    }

    private static T? _ConvertResponse<T>(string response)
    {
        if (response.Equals(""))
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(response);
    }
}

