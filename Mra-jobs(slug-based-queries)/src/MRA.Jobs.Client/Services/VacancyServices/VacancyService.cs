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

    public List<JobVacancyListDTO> Vacanceies { get; set; }

    public CreateJobVacancyCommand creatingNewJob { get; set; }

    public event Action OnChange;
    public string guidId { get; set; } = string.Empty;
    public async Task<List<JobVacancyListDTO>> GetAllVacancy()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>("jobs");
        Vacanceies = result.Items;
        Console.WriteLine(Vacanceies.Count);
        return Vacanceies;
    }

    public CreateVacancyCategoryCommand creatingEntity { get; set; }

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>("categories");
        Categories = result.Items;
        return Categories;
    }

    public async Task<List<JobVacancyListDTO>> GetVacancyByTitle(string title)
    {
        var result = await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?Filters=Title@={title}");
        Vacanceies = result.Items;
        OnChange.Invoke();
        return Vacanceies;
    }

    public async Task OnSaveCreateClick()
    {
        Console.WriteLine(creatingNewJob.CategoryId);
        await _http.PostAsJsonAsync("jobs", creatingNewJob);

        Console.WriteLine(creatingNewJob.Title);
    }

    public async Task<List<JobVacancyListDTO>> GetJobs()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>("jobs");
        return result.Items;
    }

    public async Task OnDelete(string slug)
    {
        await _http.DeleteAsync($"jobs/{slug}");
    }

    public async Task<JobVacancyDetailsDTO> GetBySlug(string slug)
    {
        var result = await _http.GetFromJsonAsync<JobVacancyDetailsDTO>($"jobs/{slug}");
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
