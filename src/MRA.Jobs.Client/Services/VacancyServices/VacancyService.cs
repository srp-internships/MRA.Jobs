using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Identity;
using MudBlazor;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Client.Services.VacancyServices;

public class VacancyService(HttpClient http,ISnackbar snackbar) : IVacancyService
{
    public List<CategoryResponse> Categories { get; set; }
    
    public List<JobVacancyListDto> Vacanceies { get; set; }
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
    public string guidId { get; set; } = string.Empty;

    public async Task<List<JobVacancyListDto>> GetAllVacancy()
    {
        var result = await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>("jobs");
        Vacanceies = result.Items;
        return Vacanceies;
    }

    public int PagesCount { get; set; }
    const float PageSize = 10f;

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        var result = await http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Categories = result.Items;
        return Categories;
    }

    /* Modified and renamed version of the upper method name with a typo in the name and getting vacancies just the 20 first ones instead of all of them */
    public async Task<List<JobVacancyListDto>> GetAllVacancies()
    {
        var result = await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?PageSize={int.MaxValue}");
        return result.Items;
    }


    public async Task<List<JobVacancyListDto>> GetVacancyByTitle(string title)
    {
        var result = await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?Filters=Title@={title}");
        Vacanceies = result.Items;
        OnChange.Invoke();
        return Vacanceies;
    }

    /* Two upper methods optimized for Vacancy page and merged to one method with default parameters */
    public async Task<List<JobVacancyListDto>> GetFilteredVacancies(string title = "",
        string categoryName = "All categories", int page = 1)
    {
        PagedList<JobVacancyListDto> result;
        if (title == "")
        {
            if (categoryName == "All categories")
            {
                if (page == 1)
                {
                    FilteredVacanciesCount =
                        (await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?PageSize={int.MaxValue}"))
                        .TotalCount;
                    PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
                }

                result = await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?PageSize=10&Page={page}");
            }
            else
            {
                if (page == 1)
                {
                    FilteredVacanciesCount =
                        (await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>(
                            $"jobs?Filters=Category@={categoryName}&PageSize={int.MaxValue}")).TotalCount;
                    PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
                }

                result = await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>(
                    $"jobs?Filters=Category@={categoryName}&PageSize=10&Page={page}");
            }
        }
        else
        {
            if (page == 1)
            {
                FilteredVacanciesCount =
                    (await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>(
                        $"jobs?Filters=Title@={title}&PageSize={int.MaxValue}")).TotalCount;
                PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
            }

            result = await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>(
                $"jobs?Filters=Title@={title}&PageSize=10&Page={page}");
        }

        OnChange?.Invoke();
        return result.Items;
    }

    public async Task<HttpResponseMessage> OnSaveCreateClick()
    {
        return await http.PostAsJsonAsync("jobs", creatingNewJob);
    }

    public async Task<List<JobVacancyListDto>> GetJobs()
    {
        var result = await http.GetFromJsonAsync<PagedList<JobVacancyListDto>>("jobs");
        return result.Items;
    }

    public async Task OnDelete(string slug)
    {
        await http.DeleteAsync($"jobs/{slug}");
    }

    public async Task<JobVacancyDetailsDto> GetBySlug(string slug)
    {
        var result = await http.GetFromJsonAsync<JobVacancyDetailsDto>($"jobs/{slug}");
        return result;
    }

    public async Task UpdateJobVacancy(string slug)
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
        await http.PutAsJsonAsync($"jobs/{slug}", update);
    }

    public async Task<List<InternshipVacancyListResponse>> GetInternship()
    {
        var result = await http.GetFromJsonAsync<PagedList<InternshipVacancyListResponse>>("internships");
        return result.Items;
    }

    public async Task<List<TrainingVacancyListDto>> GetTrainings()
    {
        var result = await http.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>("trainings");
        return result.Items;
    }
}