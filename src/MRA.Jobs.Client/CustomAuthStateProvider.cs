using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using MRA.Identity.Application.Contract.Admin.Responses;

namespace MRA.Jobs.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _http;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
    {
        _localStorageService = localStorageService;
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await _localStorageService.GetItemAsync<JwtTokenResponse>("authToken");

        var identity = new ClaimsIdentity();
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(authToken.AccessToken))
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken.AccessToken), "jwt");
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken.AccessToken.Replace("\"", ""));
            }
            catch
            {
                await _localStorageService.RemoveItemAsync("authToken");
                identity = new ClaimsIdentity();
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer
            .Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

        return claims;
    }
}
