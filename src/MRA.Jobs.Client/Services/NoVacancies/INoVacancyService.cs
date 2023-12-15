using MRA.Jobs.Application.Contracts.NoVacancies.Responses;

namespace MRA.Jobs.Client.Services.NoVacancies;

public interface INoVacancyService
{
    Task<NoVacancyResponse> GetHiddenVacancy();
    Task<ApplicationWithNoVacancyStatus> GetApplicationStatus();
}