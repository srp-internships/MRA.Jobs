namespace MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;

public class ChangeVacancyNoteCommand : IRequest<bool>
{
    public Guid VacancyId { get; set; }
    public string Note { get; set; }
}