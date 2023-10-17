using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;
using Mra.Shared.Common.Constants;
using MRA.Identity.Application.Common.Exceptions;

namespace MRA.Identity.Application.Features.UserRoles.Commands;

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRolesCommand, string>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateUserRoleCommandHandler(RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager, IApplicationDbContext applicationDbContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<string> Handle(CreateUserRolesCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId,
            cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");

        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName == request.RoleName.ToUpper(),
            cancellationToken: cancellationToken);
        if (role == null)
        {
            role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = request.RoleName,
                NormalizedName = request.RoleName.ToUpper(),
                Slug = request.RoleName.ToLower(),
            };
            await _roleManager.CreateAsync(role);//todo check identity result
        }

        var newUserRole = new ApplicationUserRole
        {
            UserId = request.UserId,
            RoleId = role.Id,
            Slug = $"{user.UserName}-{role.Slug}"
        };
        await _applicationDbContext.UserRoles.AddAsync(newUserRole, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);


        //add role claim
        var roleClaim = new ApplicationUserClaim
        {
            UserId = user.Id,
            ClaimType = ClaimTypes.Role,
            ClaimValue = role.Name,
            Slug = $"{user.UserName}-role"
        };
        await _applicationDbContext.UserClaims.AddAsync(roleClaim, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //add role claim

        return newUserRole.Slug;
    }
}