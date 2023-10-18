using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Claim.Commands;

namespace MRA.Identity.Application.Features.Claims.Commands;

public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateClaimCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.UserClaims.SingleOrDefaultAsync(s => s.Slug == request.Slug.Trim().ToLower(), cancellationToken: cancellationToken);
        _ = claim ?? throw new NotFoundException($"claim with slug {request.Slug} not found");

        claim.ClaimValue = request.ClaimValue;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;

    }
}