namespace MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;

public class UpdateApplicationStatusCommandValidator : AbstractValidator<UpdateApplicationStatusCommand>
{
    public UpdateApplicationStatusCommandValidator()
    {
        RuleFor(v => v.ApplicantUserName).NotEmpty();
        RuleFor(v => v.Slug).NotEmpty();
        RuleFor(v => v.StatusId).NotEmpty();
    }
}