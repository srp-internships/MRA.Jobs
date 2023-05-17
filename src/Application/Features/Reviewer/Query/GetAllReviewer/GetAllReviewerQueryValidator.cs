using MRA.Jobs.Application.Contracts.Reviewer.Queries;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetAllReviewer;

public class GetAllReviewerQueryValidator : AbstractValidator<GetReviewerByIdQuery>
{
    public GetAllReviewerQueryValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}