using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Claim.Commands;

namespace MRA.Identity.Application.Features.Claims.Commands;

public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, ApplicationResponse>
{
    private readonly IApplicationDbContext _context;

    public DeleteClaimCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationResponse> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var claim = await _context.UserClaims.SingleOrDefaultAsync(s => String.Equals(s.Slug.Trim(), request.Slug.Trim(), StringComparison.CurrentCultureIgnoreCase), cancellationToken: cancellationToken);
            if (claim==null)
            {
                return new ApplicationResponseBuilder().Success(false)
                    .SetErrorMessage($"claim with slug {request.Slug} not found").Build();
            }

            _context.UserClaims.Remove(claim);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return new ApplicationResponseBuilder().Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder().Success(false).SetException(e).Build();
        }
    }
}