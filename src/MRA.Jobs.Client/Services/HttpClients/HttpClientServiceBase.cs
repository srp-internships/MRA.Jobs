using System.Net.Http.Headers;
using AltairCA.Blazor.WebAssembly.Cookie;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Client.Services.HttpClients;

public abstract class HttpClientServiceBase(IHttpClientFactory httpClientFactory, IAltairCABlazorCookieUtil cookieUtil,
    IConfiguration configuration) : IHttpClientServiceBase
{
    protected abstract string ConfigKey { get; }
    private string BaseAddress => configuration[ConfigKey];
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
            using var httpClient = await CreateHttpClient();
            var response = await httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
                return ApiResponse.BuildSuccess();
            return ApiResponse.BuildFailed("Error on sending response. Please try again later", response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse.BuildFailed($"Server is not responding. {ex.Message}", ex.StatusCode);
        }
    }

    public async Task<ApiResponse<T>> PostAsJsonAsync<T>(string url, Object content)
    {
        try
        {
            using var httpClient = await CreateHttpClient();
            var response = await httpClient.PostAsJsonAsync(url, content);
            return await GetApiResponseAsync<T>(response);
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse<T>.BuildFailed($"Server is not responding. {ex.Message}", ex.StatusCode);
        }
    }

    private async Task<ApiResponse<T>> GetApiResponseAsync<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            if (typeof(T) == typeof(string))
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<T>.BuildSuccess((T)Convert.ChangeType(responseContent, typeof(T)), response.StatusCode);
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<T>();
                return ApiResponse<T>.BuildSuccess(responseContent, response.StatusCode);
            }
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
            using var httpClient = await CreateHttpClient();
            var response = await httpClient.PutAsJsonAsync(url, content);
            return await GetApiResponseAsync<T>(response);
        }
        catch (HttpRequestException ex)
        {
            return ApiResponse<T>.BuildFailed($"Server is not responding. {ex.Message}", ex.StatusCode);
        }
    }

    private async Task<HttpClient> CreateHttpClient()
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress= new Uri(BaseAddress);
        JwtTokenResponse authToken = await cookieUtil.GetValueAsync<JwtTokenResponse>("authToken");
        if (authToken!=null)
            httpClient.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", authToken.AccessToken.Replace("\"", ""));
        return httpClient;
    }
}
