using Blazored.LocalStorage;

namespace MRA.Jobs.SSR.Client.Services.HttpClients;

public class IdentityApiHttpClientService(IHttpClientFactory httpClientFactory,  ILocalStorageService cookieUtil,
    IConfiguration configuration) : HttpClientServiceBase(httpClientFactory, cookieUtil, configuration)
{
    protected override string ConfigKey => "IdentityHttpClient:BaseAddress";
}