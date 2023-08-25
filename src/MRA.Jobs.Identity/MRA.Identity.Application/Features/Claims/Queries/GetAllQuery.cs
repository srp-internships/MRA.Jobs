using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Claim.Queries;
using MRA.Identity.Application.Contract.Claim.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Claims.Queries;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ApplicationResponse<List<UserClaimsResponse>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public GetAllQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ApplicationResponse<List<UserClaimsResponse>>> Handle(GetAllQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var usersQuery = _userManager.Users;
            if (request.Username != null)
            {
                usersQuery = usersQuery.Where(s => s.NormalizedUserName == request.Username.Trim().ToLower());
            }

            var users = await usersQuery.ToArrayAsync(cancellationToken: cancellationToken);

            IQueryable<ApplicationUserClaim> claimsQuery = _context.UserClaims;
            if (request.ClaimType != null)
            {
                claimsQuery = claimsQuery.Where(s => s.ClaimType!.Contains(request.ClaimType));
            }

            var result = (from applicationUser in users
                let claims = claimsQuery.Where(s => s.UserId == applicationUser.Id)
                from claim in claims
                select new UserClaimsResponse
                {
                    Username = applicationUser.UserName!,
                    ClaimType = claim.ClaimType!,
                    ClaimValue = claim.ClaimValue!,
                    Slug = claim.Slug
                }).ToList();

            return new ApplicationResponseBuilder<List<UserClaimsResponse>>().SetResponse(result).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<List<UserClaimsResponse>>().SetException(e).Success(false).Build();
        }
    }
}