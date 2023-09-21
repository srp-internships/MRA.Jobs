using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;


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
        HttpResponseMessage response = await _httpClient.GetAsync($"api/applications/{status}");
      
        List<ApplicationListStatus> result = await response.Content.ReadFromJsonAsync<List<ApplicationListStatus>>();
        return result;
    }


    public async Task CreateApplication(CreateApplicationCommand application)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/applications", application);
    }

    public async Task<List<ApplicationListDto>> GetAllApplications()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/applications/");
        List<ApplicationListDto> result = await response.Content.ReadFromJsonAsync<List<ApplicationListDto>>();
        return result;
    }
}