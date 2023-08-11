using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands;
using Newtonsoft.Json;

namespace MRA.Jobs.Infrastructure.Services;

public class TestHttpClientService : ITestHttpClientService
{
    private readonly IConfiguration _configuration;

    public TestHttpClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<TestInfoDto> SendTestCreationRequest(CreateTestCommand request)
    {
        using HttpClient client = new();
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        string path = _configuration.GetSection("UrlSettings:DefaultUrl").Value;

        HttpResponseMessage response = await client.PostAsync(path + "api/test/create", content);

        if (response.IsSuccessStatusCode)
        {
            TestInfoDto responseContent = await response.Content.ReadFromJsonAsync<TestInfoDto>();
            return responseContent;
        }

        throw new InvalidOperationException("Request to server failed.");
    }

    public async Task<TestResultDto> GetTestResultRequest(CreateTestResultCommand request)
    {
        using HttpClient client = new();
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        string path = _configuration.GetSection("UrlSettings:DefaultUrl").Value;

        HttpResponseMessage response = await client.PostAsync(path + "api/test/pass", content);

        if (response.IsSuccessStatusCode)
        {
            TestResultDto responseContent = await response.Content.ReadFromJsonAsync<TestResultDto>();
            return responseContent;
        }

        throw new InvalidOperationException("Request to server failed.");
    }
}