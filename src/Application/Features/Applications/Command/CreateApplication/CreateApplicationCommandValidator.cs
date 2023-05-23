using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;
public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
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
