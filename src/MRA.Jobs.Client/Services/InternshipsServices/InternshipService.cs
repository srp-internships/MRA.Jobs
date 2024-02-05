using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Client.Components.Dialogs;
using MudBlazor;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public class InternshipService(
    IHttpClientService httpClientService,
    IConfiguration configuration,
    IDialogService dialogService,
    ISnackbar snackbar)
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
        var parameters = new DialogParameters<DialogAddNote>();
        parameters.Add(d => d.Note, vacancy.Note);
        if (!vacancy.Note.IsNullOrEmpty())
            parameters.Add(d => d.ShowNote, true);

        var dialog = await dialogService.ShowAsync<DialogAddNote>($"Note {vacancy.Title}", parameters,
            new(){MaxWidth = MaxWidth.Large});

        var result = await dialog.Result;
        if (result.Canceled) return;
        var note = result.Data.ToString();
        if (note.IsNullOrEmpty()) return;

        try
        {
            ChangeVacancyNoteCommand command = new() { VacancyId = vacancy.Id, Note = note };
            var response =
                await httpClientService.PutAsJsonAsync<bool>(configuration.GetJobsUrl("Vacancies/ChangeNote"), command);
            if (response.Success)
            {
                snackbar.Add("Success", Severity.Success);
                vacancy.Note = note;
            }
            else
                snackbar.Add(response.Error, Severity.Error);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public async Task<ApiResponse> Create()
    {
        return await httpClientService.PostAsJsonAsync<string>(configuration.GetJobsUrl("internships"), createCommand);
    }

    public async Task<ApiResponse> Delete(string slug)
    {
        return await httpClientService.DeleteAsync(configuration.GetJobsUrl($"internships/{slug}"));
    }

    public async Task<ApiResponse<PagedList<InternshipVacancyListResponse>>> GetAll()
    {
        var response =
            await httpClientService.GetAsJsonAsync<PagedList<InternshipVacancyListResponse>>(
                configuration.GetJobsUrl("internships"));
        return response;
    }

    public async Task<InternshipVacancyResponse> GetBySlug(string slug)
    {
        var response =
            await httpClientService.GetAsJsonAsync<InternshipVacancyResponse>(
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