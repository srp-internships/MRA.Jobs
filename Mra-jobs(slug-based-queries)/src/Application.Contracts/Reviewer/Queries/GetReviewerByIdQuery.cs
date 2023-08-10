using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Contracts.Reviewer.Queries;

public class GetReviewerByIdQuery : IRequest<ReviewerDetailsDto>
{
    public Guid Id { get; set; }
}