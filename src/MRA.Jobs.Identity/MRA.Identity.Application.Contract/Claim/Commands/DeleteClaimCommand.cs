using MediatR;

namespace MRA.Identity.Application.Contract.Claim.Commands;

#nullable disable
public class DeleteClaimCommand:IRequest<ApplicationResponse>
{
    public string Slug { get; set; }
}