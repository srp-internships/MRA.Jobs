using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Client.Services.VacanciesServices;

public interface IVacancyService
{
    Task<bool> AddTagsToVacancyAsync(AddTagsToVacancyCommand command);
    Task<List<string>> RemoveTagsFromVacancyAsync(RemoveTagsFromVacancyCommand command);
}