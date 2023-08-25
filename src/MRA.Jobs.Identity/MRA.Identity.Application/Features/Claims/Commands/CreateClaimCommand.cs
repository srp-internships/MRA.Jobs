using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Claim.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Claims.Commands;

public class CreateClaimCommandHandler:IRequestHandler<CreateClaimCommand,ApplicationResponse<Guid>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    public CreateClaimCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ApplicationResponse<Guid>> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return new ApplicationResponseBuilder<Guid>().Success(false).SetErrorMessage("user is not found").Build();
            var claim = new ApplicationUserClaim
            {
                UserId = user.Id,
                ClaimType = request.ClaimType,
                ClaimValue = request.ClaimValue,
                Slug = user.UserName + "-" + request.ClaimType
            };
            await _context.UserClaims.AddAsync(claim, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            return new ApplicationResponseBuilder<Guid>().Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<Guid>().Success(false).SetException(e).Build();
        }
    }
}