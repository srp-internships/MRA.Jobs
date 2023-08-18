using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
namespace MRA.Jobs.Client.Services.VacancyServices;

public class VacancyService : IVacancyService
{
    private readonly HttpClient _http;

    public VacancyService(HttpClient http)
    {
        _http = http;
        guidId = "";
        creatingNewJob = new()
        {
            Title = "",
            Description = "",
            ShortDescription = "",
            WorkSchedule = new() { },
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 0,
            EndDate = DateTime.Now,
            PublishDate = DateTime.Now,
        };
    }
    public List<CategoryResponse> Categories { get; set; }

    public List<JobVacancyListDto> Vacanceies { get; set; }


    public int FilteredVacanciesCount { get; set; } = 0;

    public CreateJobVacancyCommand creatingNewJob { get; set; }

    public event Action OnChange;
    public string guidId { get; set; } = string.Empty;


    public CreateVacancyCategoryCommand creatingEntity { get; set; }

    public int PagesCount { get; set; }
    const float PageSize = 10f;

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        var result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Categories = result.Items;
        return Categories;
    }

    public async Task OnSaveCreateClick()
    {
        Console.WriteLine(creatingNewJob.CategoryId);
        await _http.PostAsJsonAsync("jobs", creatingNewJob);

        Console.WriteLine(creatingNewJob.Title);
    }

    public async Task<List<JobVacancyListDto>> GetAllVacancy()
    {
        var result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>("jobs");
        Vacanceies = result.Items;
        Console.WriteLine(Vacanceies.Count);
        return Vacanceies;
    }
    /* Modified and renamed version of the upper method name with a typo in the name and getting vacancies just the 20 first ones instead of all of them */
    public async Task<List<JobVacancyListDto>> GetAllVacancies()
    {
        var result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?PageSize={int.MaxValue}");
        return result.Items;
    }

    public async Task<List<JobVacancyListDto>> GetJobs()
    {
        var result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>("jobs");
        return result.Items;
    }

    public async Task<List<JobVacancyListDto>> GetVacancyByTitle(string title)
    {
        var result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?Filters=Title@={title}");
        Vacanceies = result.Items;
        OnChange.Invoke();
        return Vacanceies;
    }

    /* Two upper methods optimized for Vacancy page and merged to one method with default parameters */
    public async Task<List<JobVacancyListDto>> GetFilteredVacancies(string title = "", string categoryName = "All categories", int page = 1)
    {
        PagedList<JobVacancyListDto> result;
        if (title == "")
        {
            if (categoryName == "All categories")
            {
                if (page == 1)
                {
                    FilteredVacanciesCount = (await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?PageSize={int.MaxValue}")).TotalCount;
                    PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
                }
                result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?PageSize=10&Page={page}");
            }
            else
            {
                if (page == 1)
                {
                    FilteredVacanciesCount = (await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?Filters=Category@={categoryName}&PageSize={int.MaxValue}")).TotalCount;
                    PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
                }
                result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?Filters=Category@={categoryName}&PageSize=10&Page={page}");
            }
        }
        else
        {
            if (page == 1)
            {
                FilteredVacanciesCount = (await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?Filters=Title@={title}&PageSize={int.MaxValue}")).TotalCount;
                PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
            }

            result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?Filters=Title@={title}&PageSize=10&Page={page}");
        }
        OnChange?.Invoke();
        return result.Items;
    }

    public async Task OnDelete(string slug)
    {
        await _http.DeleteAsync($"jobs/{slug}");
    }

    public async Task<JobVacancyDetailsDto> GetBySlug(string slug)
    {
        var result = await _http.GetFromJsonAsync<JobVacancyDetailsDto>($"jobs/{slug}");
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
        };
        await _http.PutAsJsonAsync($"jobs/{slug}", update);

    }
}
