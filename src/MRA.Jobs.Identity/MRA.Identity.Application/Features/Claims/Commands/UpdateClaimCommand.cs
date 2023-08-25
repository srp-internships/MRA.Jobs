using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Claim.Commands;

namespace MRA.Identity.Application.Features.Claims.Commands;

public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, ApplicationResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateClaimCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationResponse> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var claim = await _context.UserClaims.SingleOrDefaultAsync(s => s.Slug == request.Slug.Trim().ToLower(), cancellationToken: cancellationToken);
            if (claim==null)
            {
                return new ApplicationResponseBuilder().Success(false)
                    .SetErrorMessage($"claim with slug {request.Slug} not found").Build();
            }
            
            claim.ClaimValue = request.ClaimValue;

            await _context.SaveChangesAsync(cancellationToken);
            
            return new ApplicationResponseBuilder().Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder().Success(false).SetException(e).Build();
        }
    }
}