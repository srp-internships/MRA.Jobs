using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Queries;
using MRA.Identity.Application.Contract.UserRoles.Response;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Queries;
public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, ApplicationResponse<List<UserRolesResponse>>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    private readonly IApplicationDbContext _context;

    public async Task<ApplicationResponse<List<UserRolesResponse>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {

        if (request.UserName != null)
        {
            var user = _userManager.Users.FirstOrDefault(s => s.UserName.Contains(request.UserName));
            if (user == null)
                return new ApplicationResponseBuilder<List<UserRolesResponse>>().SetErrorMessage("User not found").Success(false).Build();


            var userRoles = await _userManager.GetRolesAsync(user);

            if (request.Role != null)
            {
                userRoles = userRoles.Where(s => s.Contains(request.Role)).ToList();
            }

            return new ApplicationResponseBuilder<List<UserRolesResponse>>()
                .SetResponse(userRoles.Select(s =>
                new UserRolesResponse
                {
                    RoleName = s,
                    UserName = user.UserName
                }).ToList());
        }
        if (request.Role != null)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(s => s.NormalizedName.Contains(request.Role));
            if (role == null)
                return new ApplicationResponseBuilder<List<UserRolesResponse>>().SetErrorMessage("role not found").Success(false).Build();
            var userRoles = await _context.UserRoles.Where(s => s.RoleId == role.Id).ToListAsync();
            var responses = new List<UserRolesResponse>();
            foreach (var userRole in userRoles)
            {
                var response = new UserRolesResponse()
                {
                    RoleName = role.Name,
                    UserName = (await _userManager.FindByIdAsync(userRole.UserId.ToString())).UserName
                };
                responses.Add(response);
            }
            return new ApplicationResponseBuilder<List<UserRolesResponse>>().SetResponse(responses).Build();
        }
        var users = await _userManager.Users.ToListAsync();


        var allResponses = new List<UserRolesResponse>();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            allResponses.AddRange(userRoles.Select(s => new UserRolesResponse { RoleName = s, UserName = user.UserName }));
        }

        return new ApplicationResponseBuilder<List<UserRolesResponse>>().SetResponse(allResponses).Build();
    }
}
