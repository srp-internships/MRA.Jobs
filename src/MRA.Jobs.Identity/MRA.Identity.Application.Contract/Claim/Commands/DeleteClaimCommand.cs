using MediatR;

namespace MRA.Identity.Application.Contract.Claim.Commands;

#nullable disable
public class DeleteClaimCommand:IRequest<Unit>
{
    public string Slug { get; set; }
}