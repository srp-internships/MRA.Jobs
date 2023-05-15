using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.CreateApplicant;

public class CreateApplicantCommandValidator : AbstractValidator<CreateApplicantCommand>
{
    public CreateApplicantCommandValidator()
    {
        RuleFor(a => a.FirstName).NotEmpty();
        RuleFor(a => a.LastName).NotEmpty();
        RuleFor(a => a.Email).NotEmpty();
        RuleFor(a => a.Patronymic).NotEmpty();
        RuleFor(a => a.BirthDay).NotEmpty();
        RuleFor(a => a.PhoneNumber).NotEmpty();
        
    }
}