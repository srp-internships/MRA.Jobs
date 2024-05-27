using MRA.Jobs.Application.Contracts.Vacancies.Responses;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Client.Services.VacanciesServices;

public interface IVacancyService
{
    Task<bool> AddTagsToVacancyAsync(AddTagsToVacancyCommand command);
    Task<List<string>> RemoveTagsFromVacancyAsync(RemoveTagsFromVacancyCommand command);
    Task<List<string>> DialogChangeTagsAsync(Guid vacancyId, List<string> tags, string vacancyTitle);
    Task<List<VacancyDto>> GetAllVacancies();
    Task<List<string>> GetAllTagsAsync();
}