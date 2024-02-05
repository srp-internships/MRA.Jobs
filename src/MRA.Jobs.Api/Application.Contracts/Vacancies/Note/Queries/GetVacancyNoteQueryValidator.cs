namespace MRA.Jobs.Application.Contracts.Vacancies.Note.Queries;

public class GetVacancyNoteQueryValidator : AbstractValidator<GetVacancyNoteQuery>
{
    public GetVacancyNoteQueryValidator()
    {
        RuleFor(v => v.VacancyId).NotEmpty();
    }
}