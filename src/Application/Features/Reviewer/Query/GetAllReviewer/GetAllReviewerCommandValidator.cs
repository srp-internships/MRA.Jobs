using MRA.Jobs.Application.Contracts.Reviewer.Queries;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetAllReviewer;

public class GetAllReviewerCommandValidator : AbstractValidator<GetReviewerByIdQuery>
{
    public GetAllReviewerCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}