namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Tags;
public class AddTagToInternshipVacancyCommandValidator : AbstractValidator<AddTagToInternshipVacancyCommand>
{
    public AddTagToInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.InternshipSlug).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}