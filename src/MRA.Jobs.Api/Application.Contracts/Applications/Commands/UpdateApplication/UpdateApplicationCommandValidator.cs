namespace MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplication;

public class UpdateApplicationCommandValidator : AbstractValidator<UpdateApplicationCommand>
{
    public UpdateApplicationCommandValidator()
    {
        RuleFor(v => v.CoverLetter)
            .NotEmpty()
            .MinimumLength(150);
    }
}