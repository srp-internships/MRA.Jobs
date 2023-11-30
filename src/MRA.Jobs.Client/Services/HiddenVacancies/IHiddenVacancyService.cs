using MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;

namespace MRA.Jobs.Client.Services.HiddenVacancies;

public interface IHiddenVacancyService
{
    Task<HiddenVacancyResponse> GetHiddenVacancy();
}