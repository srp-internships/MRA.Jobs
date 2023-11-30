using MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;
using MRA.Jobs.Client.Identity;
using MudBlazor;

namespace MRA.Jobs.Client.Services.HiddenVacancies;

public class HiddenVacancyService(HttpClient httpClient, ISnackbar snackbar) : IHiddenVacancyService
{
    public async Task<HiddenVacancyResponse> GetHiddenVacancy()
    {
        var vacancy = new HiddenVacancyResponse();
        try
        {
            var response = await httpClient.GetAsync("hiddenvacancies");
            if (response.IsSuccessStatusCode)
                vacancy = await response.Content.ReadFromJsonAsync<HiddenVacancyResponse>();
            else
                snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail);
        }
        catch (Exception e)
        {
            snackbar.Add("Server is not responding, please try later", Severity.Error);
        }
        return vacancy;
    }
}