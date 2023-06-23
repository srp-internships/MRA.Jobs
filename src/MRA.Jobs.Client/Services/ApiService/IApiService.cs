namespace MRA.Jobs.Client.Services.ApiService;

public interface IApiService
{
    Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, HttpContent content = null);

}
