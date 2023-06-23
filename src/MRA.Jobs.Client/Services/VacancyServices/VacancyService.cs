using Microsoft.AspNetCore.WebUtilities;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Vacncies.Responses;
using MRA.Jobs.Client.Services.ApiService;

namespace MRA.Jobs.Client.Services.VacancyServices;

public class VacancyService : IVacancyService
{
    private readonly HttpClient _http;

    public VacancyService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CategoryVacancyCountDTO>> GetCategories()
    {
        return await _http.GetFromJsonAsync<List<CategoryVacancyCountDTO>>("vacancies/categoryVacancyCounts");
    }

    public async Task<PaggedList<VacancyListDTO>> GetVacancies(PaggedListQuery<VacancyListDTO> query)
    {
        var response = await _http.GetAsync($"vacancies");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PaggedList<VacancyListDTO>>();
    }

    public async Task<PaggedList<VacancyListDTO>> GetVacanciesByCategory(PaggedListVacancyByCategory<VacancyListDTO> query)
    {
        var queryParams = new Dictionary<string, string>
    {
        { nameof(query.CategoryId), query.CategoryId.ToString() }
    };
        var url = QueryHelpers.AddQueryString("vacancies/byCategory", queryParams);
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PaggedList<VacancyListDTO>>();
    }

   
}
