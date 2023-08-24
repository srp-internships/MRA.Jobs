using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Client.Services.ApplicationService;

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;

    public ApplicationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetailsDto(string slug)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"application/{slug}");
        response.EnsureSuccessStatusCode();
        ApplicationDetailsDto result = await response.Content.ReadFromJsonAsync<ApplicationDetailsDto>();
        return result;
    }

    public async Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"applications?status={status}");
        response.EnsureSuccessStatusCode();
        List<ApplicationListStatus> result = await response.Content.ReadFromJsonAsync<List<ApplicationListStatus>>();
        return result;
    }

}