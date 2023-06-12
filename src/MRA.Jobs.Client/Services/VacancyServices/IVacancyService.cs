using MRA.Jobs.Application.Contracts.Vacncies.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    Task< List<CategoryVacancyCountDTO>> GetCategories();
}
