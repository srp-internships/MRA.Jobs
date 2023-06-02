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
    public List<CategoryResponse> Category { get; set; }
    public CreateJobVacancyCommand creatingNewJob { get; set; }
    public CreateVacancyCategoryCommand creatingEntity { get; set; }
    public string guidId { get; set; } = string.Empty;

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>("categories");
        Category = result.Items;
        return Category;
    }
    public async Task OnSaveCreateClick()
    {
        Console.WriteLine(creatingNewJob.CategoryId);
        await _http.PostAsJsonAsync("jobs", creatingNewJob);
        Console.WriteLine(creatingNewJob.Title);
        Console.WriteLine("shud");
    }

    public async Task<List<JobVacancyListDTO>> GetJobs()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>("jobs");
        return result.Items;
    }
}
