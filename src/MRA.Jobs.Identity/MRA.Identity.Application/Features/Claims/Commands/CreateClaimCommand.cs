using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Claim.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Claims.Commands;

public class CreateClaimCommandHandler:IRequestHandler<CreateClaimCommand,Guid>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    public CreateClaimCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Guid> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            _ = user ?? throw new NotFoundException("user is not found");
            var claim = new ApplicationUserClaim
            {
                UserId = user.Id,
                ClaimType = request.ClaimType,
                ClaimValue = request.ClaimValue,
                Slug = user.UserName + "-" + request.ClaimType
            };
            await _context.UserClaims.AddAsync(claim, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            return user.Id;

    }
}