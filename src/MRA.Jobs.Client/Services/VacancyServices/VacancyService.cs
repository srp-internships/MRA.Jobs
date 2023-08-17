using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
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
    public List<JobVacancyListDTO> VacanciesOnPage { get; set; }
    public int FilteredVacanciesCount { get; set; }
    public List<JobVacancyListDTO> AllVacancies { get; set; }
    public CreateJobVacancyCommand creatingNewJob { get; set; }

    public event Action OnChange;
    public string guidId { get; set; } = string.Empty;

    public CreateVacancyCategoryCommand creatingEntity { get; set; }

    public int PagesCount { get; set; } = 0;
    const float PageSize = 10f;

    public int VacanciesCountPerCategory(string categoryName)
    {
        return AllVacancies != null
            ? categoryName == "All categories" ? AllVacancies.Count : AllVacancies.Where(v => v.Category == categoryName).Count()
            : 0;
    }

    public async Task InitAllVacancies()
    {
        AllVacancies = (await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?PageSize={int.MaxValue}")).Items;
        OnChange?.Invoke();
    }


    public async Task<List<JobVacancyListDTO>> GetFilteredVacancies(string title = "", string categoryName = "All categories", int page = 1)
    {
        PaggedList<JobVacancyListDTO> result;
        if (title == "")
        {
            if (categoryName == "All categories")
            {
                if (page == 1)
                {
                    FilteredVacanciesCount = (await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?PageSize={int.MaxValue}")).TotalCount;
                    PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
                }
                result = await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?PageSize=10&Page={page}");
            }
            else
            {
                if (page == 1)
                {
                    FilteredVacanciesCount = (await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?Filters=Category@={categoryName}&PageSize={int.MaxValue}")).TotalCount;
                    PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
                }
                result = await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?Filters=Category@={categoryName}&PageSize=10&Page={page}");
            }
        }
        else
        {
            if (page == 1)
            {
                FilteredVacanciesCount = (await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?Filters=Title@={title}&PageSize={int.MaxValue}")).TotalCount;
                PagesCount = (int)Math.Ceiling(FilteredVacanciesCount / PageSize);
            }

            result = await _http.GetFromJsonAsync<PaggedList<JobVacancyListDTO>>($"jobs?Filters=Title@={title}&PageSize=10&Page={page}");
        }
        VacanciesOnPage = result.Items;
        OnChange?.Invoke();
        return VacanciesOnPage;
    }

    public async Task OnSaveCreateClick()
    {
        Console.WriteLine(creatingNewJob.CategoryId);
        await _http.PostAsJsonAsync("jobs", creatingNewJob);

        Console.WriteLine(creatingNewJob.Title);
    }

    public async Task OnDelete(Guid Id)
    {
        await _http.DeleteAsync($"jobs/{Id}");
    }

    public async Task<JobVacancyDetailsDTO> GetById(Guid Id)
    {
        var result = await _http.GetFromJsonAsync<JobVacancyDetailsDTO>($"jobs/{Id}");
        return result;
    }

    public async Task UpdateJobVacancy(Guid id)
    {
        var update = new UpdateJobVacancyCommand
        {
            Id = id,
            Title = creatingNewJob.Title,
            ShortDescription = creatingNewJob.ShortDescription,
            Description = creatingNewJob.Description,
            WorkSchedule = creatingNewJob.WorkSchedule,
            CategoryId = creatingNewJob.CategoryId,
            RequiredYearOfExperience = creatingNewJob.RequiredYearOfExperience,
            EndDate = creatingNewJob.EndDate,
            PublishDate = creatingNewJob.PublishDate,
        };
        await _http.PutAsJsonAsync($"jobs/{id}", update);

    }
}