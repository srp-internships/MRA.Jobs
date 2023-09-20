namespace MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

public class
    CreateApplicationWithoutApplicantIdCommandValidator : AbstractValidator<CreateApplicationWithoutApplicantIdCommand>
{
    public CreateApplicationWithoutApplicantIdCommandValidator()
    {
        RuleFor(v => v.CoverLetter)
            .NotEmpty()
            .MinimumLength(150);
        RuleFor(v => v.VacancyId).NotEmpty();
    }
}