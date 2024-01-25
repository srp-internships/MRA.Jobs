using MediatR;

namespace MRA.Identity.Application.Contract.Claim.Commands;

#nullable disable
public class CreateClaimCommand:IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}