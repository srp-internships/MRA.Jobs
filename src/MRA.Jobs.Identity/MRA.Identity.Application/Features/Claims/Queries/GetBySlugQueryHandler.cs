using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Claim.Queries;
using MRA.Identity.Application.Contract.Claim.Responses;

namespace MRA.Identity.Application.Features.Claims.Queries;

public class GetBySlugQueryHandler:IRequestHandler<GetBySlugQuery,ApplicationResponse<UserClaimsResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetBySlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationResponse<UserClaimsResponse>> Handle(GetBySlugQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var claim = await _context.UserClaims.SingleOrDefaultAsync(s => s.Slug == request.Slug.Trim().ToLower(),
                cancellationToken);
            if (claim == null)
            {
                return new ApplicationResponseBuilder<UserClaimsResponse>().SetErrorMessage("claim not found")
                    .Success(false).Build();
            }
            
            return new ApplicationResponseBuilder<UserClaimsResponse>().SetResponse(new UserClaimsResponse
            {
                Username = claim.Slug.Split('-').First(),
                ClaimType = claim.ClaimType!,
                ClaimValue = claim.ClaimValue!,
                Slug = claim.Slug
            }).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<UserClaimsResponse>().SetException(e).Success(false).Build();
        }
    }
}