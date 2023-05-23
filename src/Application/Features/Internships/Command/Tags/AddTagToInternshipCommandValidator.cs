using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.Internships.Command.Tags;
public class AddTagToInternshipCommandValidator : AbstractValidator<AddTagToInternshipCommand>
{
    public AddTagToInternshipCommandValidator()
    {
        RuleFor(x => x.InternshipId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}
