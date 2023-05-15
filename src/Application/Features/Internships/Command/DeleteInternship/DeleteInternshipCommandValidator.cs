using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.Internships.Command.DeleteInternship;
public class DeleteInternshipCommandValidator : AbstractValidator<DeleteInternshipCommand>
{
    public DeleteInternshipCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
