using MediatR;
using MRA.Identity.Application.Contract.Experiences.Queries;
using MRA.Identity.Application.Contract.Experiences.Responses;

namespace MRA.Identity.Application.Features.Experiences.Queries;
public class GetAllExperienceQueryHandler : IRequestHandler<GetAllExperienceQuery, List<UserExperienceResponse>>
{
    public Task<List<UserExperienceResponse>> Handle(GetAllExperienceQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
