using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;
using MRA.Configurations.Common.Constants;
using MRA.Identity.Application.Common.Exceptions;

namespace MRA.Identity.Application.Features.UserRoles.Commands;

public class CreateUserRoleCommandHandler(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateUserRolesCommand, string>
{
    public async Task<string> Handle(CreateUserRolesCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == request.UserName.ToUpper(),
            cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");

        var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName == request.RoleName.ToUpper(),
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
            var createResult = await roleManager.CreateAsync(role);
            if (!createResult.Succeeded)
            {
                throw new ValidationException(createResult.Errors.First().Description);
            }
        }

        var newUserRole = new ApplicationUserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            Slug = $"{user.UserName}-{role.Slug}"
        };
        await applicationDbContext.UserRoles.AddAsync(newUserRole, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return newUserRole.Slug;
    }
}