using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Components.Dialogs;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Client.Services.JobsServices;

public class JobsService(
    IHttpClientService httpClient,
    IConfiguration configuration,
    IDialogService dialogService,
    ISnackbar snackbar,
    IContentService contentService)
    : IJobsService
{
    private List<CategoryResponse> Categories { get; set; }

    public List<JobVacancyListDto> Vacancies { get; set; }
    public int FilteredVacanciesCount { get; set; } = 0;

    public CreateJobVacancyCommand creatingNewJob { get; set; } = new()
    {
        Title = "",
        Description = "",
        ShortDescription = "",
        WorkSchedule = WorkSchedule.FullTime,
        CategoryId = Guid.NewGuid(),
        RequiredYearOfExperience = 0,
        EndDate = DateTime.Now,
        PublishDate = DateTime.Now,
    };

    public event Action OnChange;

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        var result =
            await httpClient.GetFromJsonAsync<PagedList<CategoryResponse>>(configuration.GetJobsUrl("categories"));
        if (result.Success)
        {
            Categories = result.Result.Items;
            return Categories;
        }
        else
        {
            return null;
        }
    }

    public async Task<List<JobVacancyListDto>> GetVacancyByTitle(string title)
    {
        var result =
            await httpClient.GetFromJsonAsync<PagedList<JobVacancyListDto>>(
                configuration.GetJobsUrl($"jobs?Filters=Title@={title}"));
        if (result.Success)
        {
            Vacancies = result.Result.Items;
            OnChange?.Invoke();
            return Vacancies;
        }
        else
        {
            return null;
        }
    }


    public async Task<ApiResponse<string>> OnSaveCreateClick()
    {
        return await httpClient.PostAsJsonAsync<string>(configuration.GetJobsUrl("jobs"), creatingNewJob);
    }

    public async Task<List<JobVacancyListDto>> GetJobs()
    {
        var result = await httpClient.GetFromJsonAsync<PagedList<JobVacancyListDto>>(configuration.GetJobsUrl("jobs"));
        return result.Result.Items;
    }

    public async Task<ApiResponse> OnDelete(string slug)
    {
        return await httpClient.DeleteAsync(configuration.GetJobsUrl($"jobs/{slug}"));
    }

    public async Task<JobVacancyDetailsDto> GetBySlug(string slug)
    {
        var response =
            await httpClient.GetFromJsonAsync<JobVacancyDetailsDto>(configuration.GetJobsUrl($"jobs/{slug}"));
        return response.Success ? response.Result : null;
    }

    public async Task<ApiResponse<string>> UpdateJobVacancy(string slug)
    {
        var update = new UpdateJobVacancyCommand
        {
            Slug = slug,
            Title = creatingNewJob.Title,
            ShortDescription = creatingNewJob.ShortDescription,
            Description = creatingNewJob.Description,
            WorkSchedule = creatingNewJob.WorkSchedule,
            CategoryId = creatingNewJob.CategoryId,
            RequiredYearOfExperience = creatingNewJob.RequiredYearOfExperience,
            EndDate = creatingNewJob.EndDate,
            PublishDate = creatingNewJob.PublishDate,
            VacancyQuestions = creatingNewJob.VacancyQuestions,
            VacancyTasks = creatingNewJob.VacancyTasks
        };
        return await httpClient.PutAsJsonAsync<string>(configuration.GetJobsUrl($"jobs/{slug}"), update);
    }

    public async Task<List<InternshipVacancyListResponse>> GetInternship()
    {
        var result =
            await httpClient.GetFromJsonAsync<PagedList<InternshipVacancyListResponse>>(
                configuration.GetJobsUrl("internships"));
        return result.Result.Items;
    }

    public async Task<List<TrainingVacancyListDto>> GetTrainings()
    {
        var result =
            await httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>(configuration.GetJobsUrl("trainings"));
        return result.Result.Items;
    }

    public async Task ChangeNoteAsync(JobVacancyListDto vacancy)
    {
        var parameters = new DialogParameters<DialogAddNote>();
        parameters.Add(d => d.Note, vacancy.Note);
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
            await httpClient.PutAsJsonAsync<bool>(configuration.GetJobsUrl("Vacancies/ChangeNote"), command);
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"], "Success");

        if (response.Success)
        {
            vacancy.Note = note;
        }
    }
}