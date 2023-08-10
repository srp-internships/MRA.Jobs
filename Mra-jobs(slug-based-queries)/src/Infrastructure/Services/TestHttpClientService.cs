using System.Net.Http.Json;
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
    public async Task<TestInfoDTO> SendTestCreationRequest(CreateTestCommand request)
    {
        using HttpClient client = new();
        var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
        string path = _configuration.GetSection("UrlSettings:DefaultUrl").Value;

        HttpResponseMessage response = await client.PostAsync(path + "api/test/create", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<TestInfoDTO>();
            return responseContent;
        }
        else
        {
            throw new InvalidOperationException("Request to server failed.");
        }
    }

    public async Task<TestResultDTO> GetTestResultRequest(CreateTestResultCommand request)
    {
        using HttpClient client = new();
        var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
        string path = _configuration.GetSection("UrlSettings:DefaultUrl").Value;

        HttpResponseMessage response = await client.PostAsync(path + "api/test/pass", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<TestResultDTO>();
            return responseContent;
        }
        else
        {
            throw new InvalidOperationException("Request to server failed.");
        }
    }
}