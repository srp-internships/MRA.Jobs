using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Claim.Commands;

namespace MRA.Identity.Application.Features.Claims.Commands;

public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteClaimCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.UserClaims.FirstOrDefaultAsync(
            s => s.Slug.Trim() == request.Slug.Trim(),
            cancellationToken: cancellationToken);
        _ = claim ?? throw new NotFoundException($"claim with slug {request.Slug} not found");

        _context.UserClaims.Remove(claim);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}