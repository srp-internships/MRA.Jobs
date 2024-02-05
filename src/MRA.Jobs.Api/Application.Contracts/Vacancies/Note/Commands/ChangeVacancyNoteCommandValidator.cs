namespace MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;

public class ChangeVacancyNoteCommandValidator : AbstractValidator<ChangeVacancyNoteCommand>
{
    public ChangeVacancyNoteCommandValidator()
    {
        RuleFor(x => x.VacancyId).NotEmpty();
    }
}