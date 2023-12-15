using System.Net;
using MRA.Jobs.Application.Contracts.NoVacancies.Responses;
using MRA.Jobs.Client.Identity;
using MudBlazor;

namespace MRA.Jobs.Client.Services.NoVacancies;

public class NoVacancyService(HttpClient httpClient, ISnackbar snackbar) : INoVacancyService
{
    public async Task<NoVacancyResponse> GetHiddenVacancy()
    {
        var vacancy = new NoVacancyResponse();
        try
        {
            var response = await httpClient.GetAsync("hiddenvacancies");
            if (response.IsSuccessStatusCode)
                vacancy = await response.Content.ReadFromJsonAsync<NoVacancyResponse>();
            else
                snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail, Severity.Error);
        }
        catch (Exception e)
        {
            snackbar.Add("Server is not responding, please try later", Severity.Error);
        }

        return vacancy;
    }

    public async Task<ApplicationWithNoVacancyStatus> GetApplicationStatus()
    {
        ApplicationWithNoVacancyStatus status = null;
        try
        {
            var response = await httpClient.GetAsync("HiddenVacancies/GetApplicationStatus");
            if (response.IsSuccessStatusCode)
                status = await response.Content.ReadFromJsonAsync<ApplicationWithNoVacancyStatus>();
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
            }
            else
                snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail, Severity.Error);
        }
        catch (Exception e)
        {
            snackbar.Add("Server is not responding, please try later", Severity.Error);
        }

        return status;
    }
}