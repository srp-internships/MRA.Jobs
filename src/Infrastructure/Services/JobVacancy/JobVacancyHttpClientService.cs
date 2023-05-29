using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using Newtonsoft.Json;

namespace MRA.Jobs.Infrastructure.Services.JobVacancy;
public class JobVacancyHttpClientService : IJobVacancyHttpClientService
{
    private readonly IConfiguration _configuration;

    public JobVacancyHttpClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<TestInfoDTO> SendTestCreationRequest(CreateJobVacancyTestCommand request)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public Task<TestPassDTO> SendTestPassRequest(TestPassDTO request)
    {
        throw new NotImplementedException();
    }
}
