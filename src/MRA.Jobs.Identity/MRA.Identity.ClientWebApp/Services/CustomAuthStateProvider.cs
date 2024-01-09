using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;
using System.Net.Http.Headers;
using System.Security.Claims;
using Newtonsoft.Json;

namespace MRA.Identity.ClientWebApp.Services;

public class CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor, HttpClient http)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await GetTokenAsync();
        var identity = new ClaimsIdentity();
        http.DefaultRequestHeaders.Authorization = null;

        if (authToken != null && !string.IsNullOrEmpty(authToken.AccessToken))
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken.AccessToken), "jwt");
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken.AccessToken.Replace("\"", ""));
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken.AccessToken.Replace("\"", ""));
            }
            catch
            {
                httpContextAccessor.HttpContext?.Response.Cookies.Delete("authToken");
                identity = new ClaimsIdentity();
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private async Task<JwtTokenResponse> GetTokenAsync()
    {
        var cookieValue = httpContextAccessor.HttpContext?.Request.Cookies["authToken"];
        var token = cookieValue != null ? JsonConvert.DeserializeObject<JwtTokenResponse>(cookieValue) : null;
        if (token == null) return null;

        if (token.AccessTokenValidateTo <= DateTime.Now)
        {
            var refreshResponse = await http.PostAsJsonAsync("auth/refresh",
                new GetAccessTokenUsingRefreshTokenQuery
                {
                    RefreshToken = token.RefreshToken, AccessToken = token.AccessToken
                });
            if (!refreshResponse.IsSuccessStatusCode)
            {
                return null;
            }

            token = await refreshResponse.Content.ReadFromJsonAsync<JwtTokenResponse>();
            httpContextAccessor.HttpContext?.Response.Cookies.Append("authToken", JsonConvert.SerializeObject(token));
        }

        return token;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var jwtObject = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        return jwtObject.Claims;
    }
}