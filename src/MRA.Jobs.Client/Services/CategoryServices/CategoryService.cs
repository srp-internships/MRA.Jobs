using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;

namespace MRA.Jobs.Client.Services.CategoryServices;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }
    public List<GetVacancyCategoriesQuery> Category { get; set; } = new List<GetVacancyCategoriesQuery>();

    public async Task<List<GetVacancyCategoriesQuery>> GetAllCategory()
    {
        var result = await _http.GetFromJsonAsync<List<GetVacancyCategoriesQuery?>>("api/category");
        return result;
    }
}
