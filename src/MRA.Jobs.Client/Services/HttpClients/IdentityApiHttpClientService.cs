using AltairCA.Blazor.WebAssembly.Cookie;
using Blazored.LocalStorage;

namespace MRA.Jobs.Client.Services.HttpClients;

public class IdentityApiHttpClientService(IHttpClientFactory httpClientFactory,  IAltairCABlazorCookieUtil cookieUtil,
    IConfiguration configuration) : HttpClientServiceBase(httpClientFactory, cookieUtil, configuration)
{
    protected override string ConfigKey => "IdentityHttpClient:BaseAddress";
}