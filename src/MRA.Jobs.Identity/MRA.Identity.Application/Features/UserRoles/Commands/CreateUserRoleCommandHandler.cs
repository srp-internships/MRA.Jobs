using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Commands;
public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRolesCommand, ApplicationResponse<Guid>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateUserRoleCommandHandler(RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public async Task<ApplicationResponse<Guid>> Handle(CreateUserRolesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                return new ApplicationResponseBuilder<Guid>().SetErrorMessage("User not found!").Success(false).Build();
            }

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId);
            if (role == null)
            {
                return new ApplicationResponseBuilder<Guid>().SetErrorMessage("Role not found!").Success(false).Build();
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return new ApplicationResponseBuilder<Guid>().SetResponse(user.Id).Build();
            }
            return new ApplicationResponseBuilder<Guid>().SetErrorMessage(result.Errors.First().Description).Success(false).Build();

        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<Guid>().SetException(e).Success(false).Build();
        }

    }
}
