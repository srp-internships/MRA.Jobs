using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Client.Services.CategoryServices;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }
    public List<VacancyCategoryListDTO> Category { get; set; } = new List<VacancyCategoryListDTO>();

    public event Action ProductChanged;

    public async Task<PaggedList<VacancyCategoryListDTO>> GetAllCategory()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<VacancyCategoryListDTO>>("category");
        Category = result.Items;
        ProductChanged.Invoke();
        return result;
    }


}
