using MRA.BlazorComponents.HttpClient.Responses;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    Task<ApiResponse<List<string>>> AddTagsToVacancyAsync(AddTagsToVacancyCommand command);
}