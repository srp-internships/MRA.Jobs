using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;
using MudBlazor;

namespace MRA.Jobs.Client.Services.VacancyServices;

public class VacancyService(IHttpClientService httpClientService, IConfiguration configuration, ISnackbar snackbar) : IVacancyService
{
    public async Task<ApiResponse<List<string>>> AddTagsToVacancyAsync(AddTagsToVacancyCommand command)
    {
        var response =
            await httpClientService.PostAsJsonAsync<List<string>>(configuration.GetJobsUrl($"vacancies/{command.VacancyId}/tags"),
                command);
        return response.Success ? response : null;
    }
}