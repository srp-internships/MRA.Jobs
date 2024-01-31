using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Client.Services.VacancyServices;

public class VacancyService(HttpClientService httpClient, IConfiguration configuration) : IVacancyService
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
            await httpClient.GetAsJsonAsync<PagedList<CategoryResponse>>(configuration.GetJobsUrl("categories"));
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
            await httpClient.GetAsJsonAsync<PagedList<JobVacancyListDto>>(
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
        var result = await httpClient.GetAsJsonAsync<PagedList<JobVacancyListDto>>(configuration.GetJobsUrl("jobs"));
        return result.Result.Items;
    }

    public async Task<ApiResponse> OnDelete(string slug)
    {
        return await httpClient.DeleteAsync(configuration.GetJobsUrl($"jobs/{slug}"));
    }

    public async Task<JobVacancyDetailsDto> GetBySlug(string slug)
    {
        var response = await httpClient.GetAsJsonAsync<JobVacancyDetailsDto>(configuration.GetJobsUrl($"jobs/{slug}"));
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
            await httpClient.GetAsJsonAsync<PagedList<InternshipVacancyListResponse>>(
                configuration.GetJobsUrl("internships"));
        return result.Result.Items;
    }

    public async Task<List<TrainingVacancyListDto>> GetTrainings()
    {
        var result =
            await httpClient.GetAsJsonAsync<PagedList<TrainingVacancyListDto>>(configuration.GetJobsUrl("trainings"));
        return result.Result.Items;
    }
}