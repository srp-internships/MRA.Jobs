using System.Security.AccessControl;
using MRA.Jobs.Application.Contracts.Vacncies.Responses;
using static System.Net.WebRequestMethods;

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
}
