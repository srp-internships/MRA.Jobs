
namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Tags;
public class RemoveTagFromInternshipVacancyCommandValidator : AbstractValidator<RemoveTagFromInternshipVacancyCommand>
{
    public RemoveTagFromInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.InernshipSlug).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}