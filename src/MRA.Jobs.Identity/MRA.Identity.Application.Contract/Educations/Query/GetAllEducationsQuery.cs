using MediatR;
using MRA.Identity.Application.Contract.Educations.Responses;

namespace MRA.Identity.Application.Contract.Educations.Query;
public class GetAllEducationsQuery : IRequest<List<UserEducationResponse>>
{
}
