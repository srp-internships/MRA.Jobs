using System.Net;

namespace MRA.Jobs.Client.Services.AuthService;

public class GoogleAuthService : IGoogleAuthService
{

    
    public async Task<string> GetToken()
    {
        var httpClient = new HttpClient();
        var tokenEndpoint = "https://accounts.google.com/o/oauth2/token";

        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", "" },
            { "client_id", "830742943445-42hq38ijk6mhfkfu3s71s38u176rdjj7.apps.googleusercontent.com" },
            {
                "scope",
                WebUtility.UrlEncode("https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email")
            }
        };

        var response = await httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(parameters));
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}

