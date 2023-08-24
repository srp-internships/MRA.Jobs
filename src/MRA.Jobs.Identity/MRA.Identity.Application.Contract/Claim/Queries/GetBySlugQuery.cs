using MediatR;
using MRA.Identity.Application.Contract.Claim.Responses;

namespace MRA.Identity.Application.Contract.Claim.Queries;

public class GetBySlugQuery:IRequest<ApplicationResponse<UserClaimsResponse>>
{
    public string Slug { get; set; } = "";
}