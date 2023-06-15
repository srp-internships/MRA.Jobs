using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using MRA.Jobs.Client.Services.LocalStorageService;

namespace MRA.Jobs.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClient;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string accessToken = await _localStorageService.GetStringAsync("AccessToken");

        var identity = new ClaimsIdentity();
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(accessToken))
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt");
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken.Replace("\"", ""));
            }
            catch
            {
                await _localStorageService.RemoveAsync("accessToken");
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
        var keyValuePairs = JsonSerializer.
            Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = keyValuePairs.Select(kv => new Claim(kv.Key, kv.Value.ToString()));

        return claims;
    }
}
