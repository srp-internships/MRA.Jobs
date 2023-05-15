using MediatR;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Contracts.Reviewer.Queries;

public class GetReviewerByIdQuery : IRequest<ReviewerDetailsDTO>
{
    public Guid Id { get; set; }
}