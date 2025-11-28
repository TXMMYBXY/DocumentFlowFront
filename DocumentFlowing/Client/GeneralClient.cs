using DocumentFlowing.Client.Models;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentFlowing.Client;
public class GeneralClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl;
    public GeneralClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseApiUrl = "http://localhost:5189/api/";
        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
    }

    public async Task<TResponse?> UpdateResponseAsync<TRequest, TResponse>(TRequest request, string uri)
    {
        var requestJson = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync(_baseApiUrl + uri, requestContent);

        await _IsSuccessStatusCode(response);

        var responseJson = await response.Content.ReadAsStringAsync();

        return _ConvertResponse<TResponse>(responseJson);
    }

    public async Task<TResponse?> PostResponseAsync<TRequest, TResponse>(TRequest request, string uri)
    {
        var requestJson = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_baseApiUrl + uri, requestContent);

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
            RequestUri = new Uri(_baseApiUrl + uri)
        };
        requestDelete.Headers.Add("accept", "text/plain");
        var response = await _httpClient.SendAsync(requestDelete);

        await _IsSuccessStatusCode(response);

        var responseJson = await response.Content.ReadAsStringAsync();

        return _ConvertResponse<TResponse>(responseJson);
    }

    public async Task<TResponse?> GetResponseAsync<TResponse>(string uri)
    {
        var response = await _httpClient.GetAsync(_baseApiUrl + uri);

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

    class ErrorResponse
    {
        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;
    }
}

