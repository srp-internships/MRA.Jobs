using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Tags;
public class RemoveTagFromInternshipVacancyCommandValidator : AbstractValidator<RemoveTagFromInternshipVacancyCommand>
{
    public RemoveTagFromInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.InternshipId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
