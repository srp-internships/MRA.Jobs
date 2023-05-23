using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.Tags;

public class RemoveTagFromApplicantCommandValidator : AbstractValidator<RemoveTagsFromApplicantCommand>
{
    public RemoveTagFromApplicantCommandValidator()
    {
        RuleFor(x => x.ApplicantId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}