using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplication;
public class UpdateApplicationCommandValidator : AbstractValidator<UpdateApplicationCommand>
{
    public UpdateApplicationCommandValidator()
    {
        RuleFor(v => v.CoverLetter)
            .NotEmpty()
            .MinimumLength(150);
        RuleFor(v => v.CV)
            .NotEmpty();
        RuleFor(v => v.ApplicantId).NotEmpty();
        RuleFor(v => v.VacancyId).NotEmpty();
        RuleFor(v => v.StatusId).NotEmpty();
    }
}
