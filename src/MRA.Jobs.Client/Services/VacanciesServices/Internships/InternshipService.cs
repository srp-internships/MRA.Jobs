using System.Net;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Client.Components.Dialogs;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;

namespace MRA.Jobs.Client.Services.VacanciesServices.Internships;

public class InternshipService(
    IHttpClientService httpClientService,
    IConfiguration configuration,
    IDialogService dialogService,
    ISnackbar snackbar,
    IContentService contentService)
    : IInternshipService
{
    public CreateInternshipVacancyCommand createCommand { get; set; } = new()
    {
        Title = "",
        ShortDescription = "",
        Description = "",
        CategoryId = Guid.NewGuid(),
        PublishDate = DateTime.Now,
        EndDate = DateTime.Now,
        ApplicationDeadline = DateTime.Now,
        Duration = 0,
        Stipend = 0
    };

    public UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    public DeleteInternshipVacancyCommand DeleteCommand { get; set; }

    public async Task ChangeNoteAsync(InternshipVacancyListResponse vacancy)
    {
        var parameters = new DialogParameters<DialogAddNote> { { d => d.Note, vacancy.Note } };
        if (!vacancy.Note.IsNullOrEmpty())
            parameters.Add(d => d.ShowNote, true);

        var dialog = await dialogService.ShowAsync<DialogAddNote>($"Note {vacancy.Title}", parameters,
            new() { MaxWidth = MaxWidth.Large });

        var result = await dialog.Result;
        if (result.Canceled) return;
        var note = result.Data.ToString();
        if (note.IsNullOrEmpty()) return;

        ChangeVacancyNoteCommand command = new() { VacancyId = vacancy.Id, Note = note };
        var response =
            await httpClientService.PutAsJsonAsync<bool>(configuration.GetJobsUrl("Vacancies/ChangeNote"), command);
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"], "Success");
        if (response.Success)
        {
            vacancy.Note = note;
        }
    }

    public async Task<ApiResponse> Create()
    {
        return await httpClientService.PostAsJsonAsync<string>(configuration.GetJobsUrl("internships"), createCommand);
    }

    public async Task<bool> Delete(string slug)
    {
        var response = await httpClientService.DeleteAsync(configuration.GetJobsUrl($"internships/{slug}"));
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"], "Deleted");
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<PagedList<InternshipVacancyListResponse>> GetAll(GetInternshipsQueryOptions query)
    {
        var queryParam = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (!query.Tags.IsNullOrEmpty()) queryParam["Tags"] = query.Tags;
        if (!query.Filters.IsNullOrEmpty()) queryParam["Filters"] = query.Filters.Replace("Category", "Category.Name");
        if (!query.Sorts.IsNullOrEmpty()) queryParam["Sorts"] = query.Sorts;
        if (query.Page != null) queryParam["Page"] = query.Page.ToString();
        if (query.PageSize != null) queryParam["PageSize"] = query.PageSize.ToString();
        string queryString = queryParam.ToString();

        var response =
            await httpClientService.GetFromJsonAsync<PagedList<InternshipVacancyListResponse>>
                ($"{configuration.GetJobsUrl("internships")}?{queryString}");
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"]);
        return response.Success ? response.Result : null;
    }

    public async Task<InternshipVacancyResponse> GetBySlug(string slug)
    {
        var response =
            await httpClientService.GetFromJsonAsync<InternshipVacancyResponse>(
                configuration.GetJobsUrl($"internships/{slug}"));
        return response.Success ? response.Result : null;
    }

    public async Task<ApiResponse> Update(string slug)
    {
        var updateCommand = new UpdateInternshipVacancyCommand
        {
            Slug = slug,
            Title = createCommand.Title,
            ShortDescription = createCommand.ShortDescription,
            Description = createCommand.Description,
            CategoryId = createCommand.CategoryId,
            PublishDate = createCommand.PublishDate,
            EndDate = createCommand.EndDate,
            ApplicationDeadline = createCommand.ApplicationDeadline,
            Duration = createCommand.Duration,
            Stipend = createCommand.Stipend,
            VacancyQuestions = createCommand.VacancyQuestions,
            VacancyTasks = createCommand.VacancyTasks
        };

        return await httpClientService.PutAsJsonAsync<string>(configuration.GetJobsUrl($"internships/{slug}"),
            updateCommand);
    }
}