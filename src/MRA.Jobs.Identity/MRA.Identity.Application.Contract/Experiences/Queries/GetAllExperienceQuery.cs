using MediatR;
using MRA.Identity.Application.Contract.Experiences.Responses;

namespace MRA.Identity.Application.Contract.Experiences.Queries;
public class GetAllExperienceQuery :IRequest<List<UserExperienceResponse>>
{
}
