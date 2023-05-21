using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Tags;
public class RemoveTagFromInternshipCommandValidator : AbstractValidator<RemoveTagFromInternshipCommand>
{
    public RemoveTagFromInternshipCommandValidator()
    {
        RuleFor(x => x.InternshipId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
