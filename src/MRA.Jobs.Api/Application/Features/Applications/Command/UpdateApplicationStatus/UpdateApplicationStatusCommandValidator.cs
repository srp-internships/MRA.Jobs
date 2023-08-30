namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplicationStatus;

public class
    UpdateApplicationStatusCommandValidator : AbstractValidator<Contracts.Applications.Commands.UpdateApplicationStatus>
{
    public UpdateApplicationStatusCommandValidator()
    {
        RuleFor(v => v.Slug)
            .NotEmpty();
        RuleFor(v => v.StatusId)
            .NotEmpty();
    }
}