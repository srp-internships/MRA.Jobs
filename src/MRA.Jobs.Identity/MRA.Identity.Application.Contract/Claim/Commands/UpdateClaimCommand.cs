using MediatR;

namespace MRA.Identity.Application.Contract.Claim.Commands;

public class UpdateClaimCommand:IRequest<Unit>
{
    public string ClaimValue { get; set; } = "";
    public string Slug { get; set; } = "";
}