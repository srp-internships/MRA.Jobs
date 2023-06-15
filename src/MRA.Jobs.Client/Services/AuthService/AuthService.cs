using System.Net.Http.Json;
using MRA.Jobs.Infrastructure.Shared.Auth.Commands;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.Client.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }
    public async Task<JwtTokenResponse> Login(string email, string password)
    {
        var command = new BasicAuthenticationCommand
        {
            Email = email,
            Password = password
        };
        var response = await _http.PostAsJsonAsync("auth/login", command);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<JwtTokenResponse>();
        }
         throw new Exception("Неверное имя пользователя или пароль");
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }
}
