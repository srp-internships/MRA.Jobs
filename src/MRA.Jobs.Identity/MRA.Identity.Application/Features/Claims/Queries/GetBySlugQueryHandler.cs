using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Claim.Queries;
using MRA.Identity.Application.Contract.Claim.Responses;
namespace MRA.Identity.Application.Features.Claims.Queries;

public class GetBySlugQueryHandler:IRequestHandler<GetBySlugQuery,UserClaimsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetBySlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserClaimsResponse> Handle(GetBySlugQuery request, CancellationToken cancellationToken)
    {
            var claim = await _context.UserClaims.SingleOrDefaultAsync(s => s.Slug == request.Slug.Trim().ToLower(),
                cancellationToken);
            _ = claim ?? throw new NotFoundException("claim not found");     
            return new UserClaimsResponse
            {
                Username = claim.Slug.Split('-').First(),
                ClaimType = claim.ClaimType!,
                ClaimValue = claim.ClaimValue!,
                Slug = claim.Slug
            };
    }
}