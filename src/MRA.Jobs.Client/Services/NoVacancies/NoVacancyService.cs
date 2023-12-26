using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Client.Identity;
using MudBlazor;

namespace MRA.Jobs.Client.Services.NoVacancies;

public class NoVacancyService(
    ISnackbar snackbar,
    HttpClient httpClient,
    NavigationManager navigationManager,
    IConfiguration configuration) : INoVacancyService
{
    public async Task<JobVacancyDetailsDto> GetNoVacancyAsync()
    {
        var vacancy = new JobVacancyDetailsDto();
        try
        {
            var response = await httpClient.GetAsync($"jobs/{CommonVacanciesSlugs.NoVacancySlug}");
            if (response.IsSuccessStatusCode)
                vacancy = await response.Content.ReadFromJsonAsync<JobVacancyDetailsDto>();
            else
                snackbar.Add((await response.Content.ReadFromJsonAsync<CustomProblemDetails>()).Detail, Severity.Error);
        }
        catch (Exception e)
        {
            snackbar.Add("Server is not responding, please try later", Severity.Error);
            Console.WriteLine(e);
        }

        return vacancy;
    }

    public async Task CreateApplicationNoVacancyAsync(CreateApplicationCommand application, IBrowserFile file)
    {
        try
        {
            //set cv
            var fileBytes = await GetFileBytesAsync(file);
            application.Cv.IsUploadCvMode = true;
            application.VacancySlug = CommonVacanciesSlugs.NoVacancySlug;
            application.Cv.CvBytes = fileBytes;
            application.Cv.FileName = file.Name;
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
        try
        {
            if (file.Size <= int.Parse(configuration["CvSettings:MaxFileSize"]!))
            {
                var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                var res = ms.ToArray();
                return res;
            }
        }
        catch (Exception ex)
        {
            var ms = ex.Message.ToString();
            var msg = ms;
        }
        
        return null;
    }
}