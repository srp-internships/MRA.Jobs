using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.NoVacancies.Responses;
using MRA.Jobs.Client.Identity;
using MudBlazor;


namespace MRA.Jobs.Client.Services.NoVacancies;

public class NoVacancyService(HttpClient httpClient, ISnackbar snackbar, NavigationManager navigationManager,
    IConfiguration configuration) : INoVacancyService
{
    public async Task<NoVacancyResponse> GetNoVacancy()
    {
        var vacancy = new NoVacancyResponse();
        try
        {
            var response = await httpClient.GetAsync("NoVacancies");
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

    public async Task CreateApplicationNoVacancy(CreateApplicationNoVacancyCommand application, IBrowserFile file)
    {
        try
        {
            //set cv
            if (application.Cv.IsUploadCvMode)
            {
                var fileBytes = await GetFileBytesAsync(file);
                application.Cv.CvBytes = fileBytes;
                application.Cv.FileName = file.Name;
            }
            //set cv

            var response = await httpClient.PostAsJsonAsync("Applications/CreateApplicationNoVacancy", application);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    snackbar.Add("Applications sent successfully!", Severity.Success);
                    navigationManager.NavigateTo(navigationManager.Uri.Replace("/apply/", "/"));
                    break;
                case HttpStatusCode.Conflict:
                    snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail,
                        Severity.Error);
                    break;
                default:
                    snackbar.Add("Something went wrong", Severity.Error);
                    break;
            }
        }
        catch (Exception e)
        {
            snackbar.Add("Server is not responding, please try later", Severity.Error);
            Console.WriteLine(e.Message);
        }
    }
    private async Task<byte[]> GetFileBytesAsync(IBrowserFile file)
    {
        if (file.Size <= int.Parse(configuration["CvSettings:MaxFileSize"]!) * 1024 * 1024)
        {
            var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            var res = ms.ToArray();
            return res;
        }

        return null;
    }
}