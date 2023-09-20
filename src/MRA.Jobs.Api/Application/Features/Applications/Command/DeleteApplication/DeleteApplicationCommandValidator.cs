using MRA.Jobs.Application.Contracts.Applications.Commands.Delete;

namespace MRA.Jobs.Application.Features.Applications.Command.DeleteApplication;

public class DeleteApplicationCommandValidator : AbstractValidator<DeleteApplicationCommand>
{
    public DeleteApplicationCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}