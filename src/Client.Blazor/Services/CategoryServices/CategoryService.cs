using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Applications.Queries;

namespace MRA.Jobs.Client.Services.CategoryServices;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;
    public CategoryService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<GetApplicationsQuery>> GetAllCategory()
    {
        var result = await _http.GetFromJsonAsync<List<GetApplicationsQuery>>("api/");
        return result;
    }
}
