using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Client;

namespace MRA.Jobs.SSR.Client;

public class CustomAuthStateProvider(ILocalStorageService cookieUtil, HttpClient http,
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
                await cookieUtil.RemoveItemAsync("authToken");
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
        return new JwtTokenResponse();
        var token = await cookieUtil.GetItemAsync<JwtTokenResponse>("authToken");
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
            await cookieUtil.SetItemAsync("authToken", token);
        }

        return token;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var jwtObject = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        return jwtObject.Claims;
    }
}