using MRA.Jobs.Application.Contracts.NoVacancies.Responses;

namespace MRA.Jobs.Client.Services.NoVacancies;

public interface INoVacancyService
{
    Task<NoVacancyResponse> GetNoVacancy();
    Task<ApplicationWithNoVacancyStatus> GetApplicationStatus();
}