using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using AltairCA.Blazor.WebAssembly.Cookie;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Client;

public class CustomAuthStateProvider(IAltairCABlazorCookieUtil cookieUtil, HttpClient http,
        IdentityHttpClient identityHttp)
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
                identityHttp.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken.AccessToken.Replace("\"", ""));
            }
            catch
            {
                await cookieUtil.RemoveAsync("authToken");
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
        var token = await cookieUtil.GetValueAsync<JwtTokenResponse>("authToken");
        if (token == null)
        {
            return null;
        }

        if (token.AccessTokenValidateTo <= DateTime.Now)
        {
            var refreshResponse = await identityHttp.PostAsJsonAsync("auth/refresh",
                new GetAccessTokenUsingRefreshTokenQuery
                {
                    RefreshToken = token.RefreshToken, AccessToken = token.AccessToken
                });
            if (!refreshResponse.IsSuccessStatusCode)
            {
                return null;
            }

            token = await refreshResponse.Content.ReadFromJsonAsync<JwtTokenResponse>();
            await cookieUtil.SetValueAsync("authToken", token);
        }

        return token;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var jwtObject = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        return jwtObject.Claims;
    }
}