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

    public async Task<List<ApplicationListStatus>> GetApplicationsByStatus(ApplicationStatus status)
    {
        var response = await _httpClient.GetAsync($"applications/{status}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<ApplicationListStatus>>();
        return result;
    }
}
