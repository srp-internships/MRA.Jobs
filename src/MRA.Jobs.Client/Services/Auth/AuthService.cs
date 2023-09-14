using System.Net;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Commands;

namespace MRA.Jobs.Client.Services.Auth;


public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JwtTokenResponse> LoginUserAsync(LoginUserCommand command)
    {
        var result = await _httpClient.PostAsJsonAsync($"{IdentityURL.Auth}login", command);
        if (result.IsSuccessStatusCode)
        {
            var response = await result.Content.ReadFromJsonAsync<JwtTokenResponse>();
            return response;
        }
        else
        {
            return null;
        }
    }


    public async Task<bool> RegisterUserAsync(RegisterUserCommand command)
    {
        var result = await _httpClient.PostAsJsonAsync($"{IdentityURL.Auth}register", command);
        if (result.StatusCode == HttpStatusCode.OK)
            return true;
        return false;
    }
}



