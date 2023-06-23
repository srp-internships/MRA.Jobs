using Microsoft.AspNetCore.Components;
using MRA.Jobs.Client.Services.LocalStorageService;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;
using System.Net.Http.Headers;

namespace MRA.Jobs.Client.Services.ApiService;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageService _localStorage;

    public ApiService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }


    public async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, HttpContent content = null)
    {
        // Получение действующего accessToken
        var accessToken = await GetAccessTokenAsync();

        // Создание запроса с accessToken в заголовке Authorization
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        if (content != null)
            request.Content = content;

        // Отправка запроса на сервер
        var response = await _httpClient.SendAsync(request);
        return response;
    }

    private async Task<string> GetAccessTokenAsync()
    {
        // Получение accessToken и refreshToken из локального хранилища
        var accessToken = await _localStorage.GetItemAsync("accessToken");
        var refreshToken = await _localStorage.GetItemAsync("refreshToken");

        // Проверка срока действия accessToken
        var isAccessTokenExpired = await CheckIfTokenIsExpired(accessToken);
        if (isAccessTokenExpired)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                // Если refreshToken отсутствует, сохраняем текущий URL-адрес и перенаправляем пользователя на страницу входа
                var returnUrl = _navigationManager.Uri;
                await _localStorage.SaveItemAsync("returnUrl", returnUrl);
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                // Если срок действия accessToken истек, отправляем запрос на сервер с refreshToken для получения нового accessToken
                var response = await _httpClient.PostAsJsonAsync("auth/refresh", new { RefreshToken = refreshToken });
                var data = await response.Content.ReadFromJsonAsync<JwtTokenResponse>();
                // Сохраняем новый accessToken в локальном хранилище
                await _localStorage.SaveItemAsync("accessToken", data.AccessToken);
                accessToken = data.AccessToken;
            }
        }

        return accessToken;
    }

    private async Task<bool> CheckIfTokenIsExpired(string token)
    {
        var refreshTokenValidTo = await _localStorage.GetItemAsync("RefreshTokenValidTo");
        if (DateTime.Parse(refreshTokenValidTo) < DateTime.UtcNow)
        {
            return false;
        }
        return true;
    }
}
