using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;
public class UpdateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
{
    public UpdateApplicationCommandValidator()
    {
        RuleFor(v => v.CoverLetter)
            .NotEmpty()
            .MinimumLength(150);
        RuleFor(v => v.ResumeUrl)
            .NotEmpty();
        RuleFor(v => v.ApplicantId).NotEmpty();
        RuleFor(v => v.VacancyId).NotEmpty();
        RuleFor(v => v.StatusId).NotEmpty();
        RuleFor(v => v.History).NotEmpty();
    }
}
