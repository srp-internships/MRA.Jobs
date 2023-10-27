using MediatR;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Educations.Responses;

namespace MRA.Identity.Application.Features.Educations.Queries;
public class GetAddEducationsQueryHandler : IRequestHandler<GetAllEducationsQuery, List<UserEducationResponse>>
{
    public Task<List<UserEducationResponse>> Handle(GetAllEducationsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
