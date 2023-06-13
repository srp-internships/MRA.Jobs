using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Vacncies.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;


public interface IVacancyService
{
    Task<PaggedList<VacancyListDTO>> GetVacancies(PaggedListQuery<VacancyListDTO> query);
    Task<PaggedList<VacancyListDTO>> GetVacanciesByCategory(PaggedListVacancyByCategory<VacancyListDTO> query);
    Task< List<CategoryVacancyCountDTO>> GetCategories();
}
