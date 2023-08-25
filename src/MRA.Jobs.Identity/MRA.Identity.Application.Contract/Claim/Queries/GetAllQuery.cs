using MediatR;
using MRA.Identity.Application.Contract.Claim.Responses;

namespace MRA.Identity.Application.Contract.Claim.Queries;

public class GetAllQuery : IRequest<ApplicationResponse<List<UserClaimsResponse>>>
{
    public string? Username { get; set; } = null;
    public string? ClaimType { get; set; } = null;
}