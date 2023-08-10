namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Commands;
public class UpdateApplicationStatusCommandValidator : AbstractValidator<UpdateApplicationStatus>
{
    public UpdateApplicationStatusCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
        RuleFor(v => v.StatusId)
            .NotEmpty();
    }
}
