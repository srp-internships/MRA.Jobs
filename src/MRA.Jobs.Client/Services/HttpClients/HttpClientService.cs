using Blazored.LocalStorage;
using MRA.Jobs.Client.Services.Auth;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MRA.Jobs.Client.Services.HttpClients;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorageService;
    public HttpClientService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
    {
        _httpClientFactory = httpClientFactory;
        _localStorageService = localStorageService;
    }

    public async Task<ApiResponse<T>> GetAsJsonAsync<T>(string url, object content = null)
    {
        try
        {
            using var httpClient = await CreateHttpClient();
            return content == null ? ApiResponse<T>.BuildSuccess(await httpClient.GetFromJsonAsync<T>(url))
                : ApiResponse<T>.BuildSuccess(await httpClient.GetFromJsonAsync<T>(url, content));
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse<T>.BuildFailed($"Server is not responding. {ex.Message}", ex.StatusCode);
        }
    }

    public async Task<ApiResponse> DeleteAsync(string url)
    {
        try
        {
            using var _httpClient = await CreateHttpClient();
            var response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
                return ApiResponse.BuildSuccess();
            return ApiResponse.BuildFailed("Error on sending response. Please try again later", response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse.BuildFailed($"Server is not responding. {ex.Message}", ex.StatusCode);
        }
    }

    public async Task<ApiResponse<T>> PostAsJsonAsync<T>(string url, object content)
    {
        try
        {
            using var _httpClient = await CreateHttpClient();
            var response = await _httpClient.PostAsJsonAsync(url, content);
            return await GetApiResponseAsync<T>(response);
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse<T>.BuildFailed($"Server is not responding. {ex.Message}", ex.StatusCode);
        }
    }

    async Task<ApiResponse<T>> GetApiResponseAsync<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<T>();
            return ApiResponse<T>.BuildSuccess(responseContent);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            ErrorResponse responseContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResponse<T>.BuildFailed(responseContent, response.StatusCode);
        }
        return ApiResponse<T>.BuildFailed("Error on sending response. Please try again later", response.StatusCode);
    }

    public async Task<ApiResponse<T>> PutAsJsonAsync<T>(string url, object content)
    {
        try
        {
            using var _httpClient = await CreateHttpClient();
            var response = await _httpClient.PutAsJsonAsync(url, content);
            return await GetApiResponseAsync<T>(response);
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse<T>.BuildFailed($"Server is not responding. {ex.Message}", ex.StatusCode);
        }
    }

    private async Task<HttpClient> CreateHttpClient()
    {
        var _httpClient = _httpClientFactory.CreateClient();
        string token = await _localStorageService.GetItemAsync<string>(IAuthService.TokenLocalStorageKey);
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _httpClient;
    }
}
