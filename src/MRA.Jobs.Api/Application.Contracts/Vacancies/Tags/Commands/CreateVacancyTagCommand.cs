namespace MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

public class AddTagsToVacancyCommand : IRequest<List<string>>
{
    public Guid VacancyId { get; set; }
    public string[] Tags { get; set; }
}