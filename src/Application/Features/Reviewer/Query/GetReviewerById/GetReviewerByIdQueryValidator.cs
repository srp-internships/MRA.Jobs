using MRA.Jobs.Application.Contracts.Reviewer.Queries;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;

public class GetReviewerByIdQueryValidator : AbstractValidator<GetReviewerByIdQuery>
{
    public GetReviewerByIdQueryValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}