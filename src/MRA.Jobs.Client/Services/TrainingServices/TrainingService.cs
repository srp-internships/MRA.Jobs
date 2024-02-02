using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Client.Components.Dialogs;
using MudBlazor;

namespace MRA.Jobs.Client.Services.TrainingServices;

public class TrainingService(
    IHttpClientService httpClientService,
    AuthenticationStateProvider authenticationStateProvider,
    IConfiguration configuration,
    IDialogService dialogService,
    ISnackbar snackbar)
    : ITrainingService
{
    public CreateTrainingVacancyCommand createCommand { get; set; } = new()
    {
        Title = "",
        CategoryId = Guid.NewGuid(),
        Description = "",
        ShortDescription = "",
        Duration = 0,
        EndDate = DateTime.Now,
        PublishDate = DateTime.Now,
        Fees = 0
    };

    public UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    public DeleteTrainingVacancyCommand DeleteCommand { get; set; }

    public async Task ChangeNoteAsync(TrainingVacancyListDto vacancy)
    {
        var parameters = new DialogParameters<DialogAddNote>();
        parameters.Add(d => d.Note, vacancy.Note);
        parameters.Add(d => d.ShowNote, true);

        var dialog = await dialogService.ShowAsync<DialogAddNote>($"Note {vacancy.Title}", parameters);
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

    public async Task<ApiResponse<string>> Create()
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        return await httpClientService.PostAsJsonAsync<string>(configuration.GetJobsUrl("trainings"), createCommand);
    }

    public async Task<ApiResponse> Delete(string slug)
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        return await httpClientService.DeleteAsync(configuration.GetJobsUrl($"trainings/{slug}"));
    }

    public async Task<ApiResponse> Update(string slug)
    {
        UpdateCommand = new UpdateTrainingVacancyCommand
        {
            Slug = slug,
            Title = createCommand.Title,
            Description = createCommand.Description,
            Duration = createCommand.Duration,
            EndDate = createCommand.EndDate,
            PublishDate = createCommand.PublishDate,
            ShortDescription = createCommand.ShortDescription,
            CategoryId = createCommand.CategoryId,
            Fees = createCommand.Fees,
            VacancyQuestions = createCommand.VacancyQuestions,
            VacancyTasks = createCommand.VacancyTasks
        };
        return await httpClientService.PutAsJsonAsync<string>(configuration.GetJobsUrl($"trainings/{slug}"),
            UpdateCommand);
    }

    public async Task<ApiResponse<PagedList<TrainingVacancyListDto>>> GetAll()
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        var result =
            await httpClientService.GetAsJsonAsync<PagedList<TrainingVacancyListDto>>(
                configuration.GetJobsUrl("trainings"));
        return result;
    }

    public async Task<TrainingVacancyDetailedResponse> GetBySlug(string slug)
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        var response =
            await httpClientService.GetAsJsonAsync<TrainingVacancyDetailedResponse>(
                configuration.GetJobsUrl($"trainings/{slug}"));
        return response.Success ? response.Result : null;
    }
}