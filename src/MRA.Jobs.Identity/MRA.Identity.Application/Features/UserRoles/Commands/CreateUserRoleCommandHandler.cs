﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Commands;
public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRolesCommand, ApplicationResponse<string>>
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
    public async Task<ApplicationResponse<string>> Handle(CreateUserRolesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                return new ApplicationResponseBuilder<string>().SetErrorMessage("User not found!").Success(false).Build();
            }

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId);
            if (role == null)
            {
                return new ApplicationResponseBuilder<string>().SetErrorMessage("Role not found!").Success(false).Build();
            }

            var newUserRole = new ApplicationUserRole
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
                Slug = $"{user.UserName}-{role.Slug}"
            };
            await _applicationDbContext.UserRoles.AddAsync(newUserRole);
            await _applicationDbContext.SaveChangesAsync(); 
            
                return new ApplicationResponseBuilder<string>().SetResponse(newUserRole.Slug).Build();
            
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<string>().SetException(e).Success(false).Build();
        }

    }
}