using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

public class
    CreateApplicationWithoutApplicantIdCommandValidator : AbstractValidator<CreateApplicationWithoutApplicantIdCommand>
{
    public CreateApplicationWithoutApplicantIdCommandValidator()
    {
        RuleFor(v => v.CoverLetter)
            .NotEmpty()
            .MinimumLength(150);
        RuleFor(v => v.CV)
            .NotEmpty();
        RuleFor(v => v.VacancyId).NotEmpty();
    }
}