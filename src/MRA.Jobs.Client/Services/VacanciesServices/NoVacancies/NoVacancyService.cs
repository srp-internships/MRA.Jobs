using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;

namespace MRA.Jobs.Client.Services.NoVacancies;

public class NoVacancyService(
    ISnackbar snackbar,
    IHttpClientService httpClient,
    NavigationManager navigationManager,
    IConfiguration configuration,
    IContentService contentService) : INoVacancyService
{
    public async Task<JobVacancyDetailsDto> GetNoVacancyAsync()
    {
        var vacancy = new JobVacancyDetailsDto();
        var response =
            await httpClient.GetFromJsonAsync<JobVacancyDetailsDto>(
                configuration.GetJobsUrl($"jobs/{CommonVacanciesSlugs.NoVacancySlug}"));
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"]);

        if (response.Success)
            vacancy = response.Result;
        return vacancy;
    }

    public async Task CreateApplicationNoVacancyAsync(CreateApplicationCommand application, IBrowserFile file)
    {
        //set cv
        var fileBytes = await GetFileBytesAsync(file);
        application.Cv.IsUploadCvMode = true;
        application.VacancySlug = CommonVacanciesSlugs.NoVacancySlug;
        application.Cv.CvBytes = fileBytes;
        application.Cv.FileName = file.Name;
        //set cv

        var response =
            await httpClient.PostAsJsonAsync<Guid>(
                configuration.GetJobsUrl("Applications/CreateApplicationNoVacancy"), application);
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"], contentService["Application:Success"]);

        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            navigationManager.NavigateTo(navigationManager.Uri.Replace("/apply/", "/"));
        }
    }

    private async Task<byte[]> GetFileBytesAsync(IBrowserFile file)
    {
        var allowedSize = int.Parse(configuration["CvSettings:MaxFileSize"]!);
        if (file.Size <= allowedSize)
        {
            var ms = new MemoryStream();
            await file.OpenReadStream(allowedSize).CopyToAsync(ms);
            var res = ms.ToArray();
            return res;
        }

        return null;
    }
}