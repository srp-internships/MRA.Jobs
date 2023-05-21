using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.Reviewer.Command.Tags;

public class RemoveTagFromReviewerCommandValidator : AbstractValidator<RemoveTagFromJobVacancyCommand>
{
    public RemoveTagFromReviewerCommandValidator()
    {
        RuleFor(x => x.JobVacancyId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}