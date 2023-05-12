using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer.Query.GetAllReviewer;

public class GetAllReviewerCommandHandler : IRequestHandler<GetAllReviewerQuery, ReviewerListDTO>
{
    public GetAllReviewerCommandHandler()
    {
        
    }
    
    public Task<ReviewerListDTO> Handle(GetAllReviewerQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}