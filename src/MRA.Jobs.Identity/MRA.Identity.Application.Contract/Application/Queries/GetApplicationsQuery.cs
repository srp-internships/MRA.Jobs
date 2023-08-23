using MediatR;
using MRA.Identity.Application.Contract.Application.Responses;

namespace MRA.Identity.Application.Contract.Application.Queries;

public class GetApplicationsQuery :IRequest<ApplicationResponse<ApplicationNameResponse>>
{

}