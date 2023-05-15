using MRA.Jobs.Application.Contracts.Reviewer.Queries;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;

public class GetReviewerByIdCommandValidator : AbstractValidator<GetReviewerByIdQuery>
{
    public GetReviewerByIdCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}