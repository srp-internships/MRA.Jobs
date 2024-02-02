namespace MRA.Jobs.Application.Contracts.Vacancies.Note.Queries;

public class GetVacancyNoteQuery : IRequest<string>
{
    public string VacancyId { get; set; }
}