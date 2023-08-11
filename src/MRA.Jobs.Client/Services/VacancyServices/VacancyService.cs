using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Client.Services.VacancyServices;

public class VacancyService : IVacancyService
{
    private readonly HttpClient _http;

    public VacancyService(HttpClient http)
    {
        _http = http;
        guidId = "";
        creatingNewJob = new CreateJobVacancyCommand
        {
            Title = "",
            Description = "",
            ShortDescription = "",
            WorkSchedule = new WorkSchedule(),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 0,
            EndDate = DateTime.Now,
            PublishDate = DateTime.Now
        };
    }

    public CreateVacancyCategoryCommand creatingEntity { get; set; }
    public List<CategoryResponse> Categories { get; set; }

    public List<JobVacancyListDto> Vacanceies { get; set; }

    public CreateJobVacancyCommand creatingNewJob { get; set; }

    public event Action OnChange;
    public string guidId { get; set; } = string.Empty;

    public async Task<List<JobVacancyListDto>> GetAllVacancy()
    {
        PagedList<JobVacancyListDto> result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>("jobs");
        Vacanceies = result.Items;
        Console.WriteLine(Vacanceies.Count);
        return Vacanceies;
    }

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        PagedList<CategoryResponse> result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Categories = result.Items;
        return Categories;
    }

    public async Task<List<JobVacancyListDto>> GetVacancyByTitle(string title)
    {
        PagedList<JobVacancyListDto> result =
            await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>($"jobs?Filters=Title@={title}");
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

    public async Task<List<JobVacancyListDto>> GetJobs()
    {
        PagedList<JobVacancyListDto> result = await _http.GetFromJsonAsync<PagedList<JobVacancyListDto>>("jobs");
        return result.Items;
    }

    public async Task OnDelete(Guid Id)
    {
        await _http.DeleteAsync($"jobs/{Id}");
    }

    public async Task<JobVacancyDetailsDto> GetById(Guid Id)
    {
        JobVacancyDetailsDto result = await _http.GetFromJsonAsync<JobVacancyDetailsDto>($"jobs/{Id}");
        return result;
    }

    public async Task UpdateJobVacancy(Guid id)
    {
        UpdateJobVacancyCommand update = new()
        {
            Id = id,
            Title = creatingNewJob.Title,
            ShortDescription = creatingNewJob.ShortDescription,
            Description = creatingNewJob.Description,
            WorkSchedule = creatingNewJob.WorkSchedule,
            CategoryId = creatingNewJob.CategoryId,
            RequiredYearOfExperience = creatingNewJob.RequiredYearOfExperience,
            EndDate = creatingNewJob.EndDate,
            PublishDate = creatingNewJob.PublishDate
        };
        await _http.PutAsJsonAsync($"jobs/{id}", update);
    }
}