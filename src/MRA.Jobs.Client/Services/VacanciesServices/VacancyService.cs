using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;

namespace MRA.Jobs.Client.Services.VacanciesServices;

public class VacancyService(
    IHttpClientService httpClientService,
    IConfiguration configuration,
    ISnackbar snackbar,
    IContentService contentService)
    : IVacancyService
{
    public async Task<bool> AddTagsToVacancyAsync(AddTagsToVacancyCommand command)
    {
        command.Tags = command.Tags.Distinct().ToArray();
        var response = await httpClientService.PostAsJsonAsync(
            configuration.GetJobsUrl($"vacancies/{command.VacancyId}/tags"),
            command);
        if (response.Success)
        {
            snackbar.Add("Success", Severity.Success);
            return true;
        }

        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"], "Add Tags");
        return false;
    }

    public async Task<List<string>> RemoveTagsFromVacancyAsync(RemoveTagsFromVacancyCommand command)
    {
        var response = await httpClientService.PutAsJsonAsync<List<string>>(
            configuration.GetJobsUrl($"vacancies/{command.VacancyId}/tags"),
            command);
        if (response.Success)
        {
            snackbar.Add("Success", Severity.Success);
            return response.Result;
        }

        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"], "Removed");
        return null;
    }
}