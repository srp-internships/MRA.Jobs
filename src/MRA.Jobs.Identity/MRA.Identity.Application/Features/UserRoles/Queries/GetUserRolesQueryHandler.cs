using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Queries;
using MRA.Identity.Application.Contract.UserRoles.Response;
using MRA.Identity.Domain.Entities;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace MRA.Identity.Application.Features.UserRoles.Queries;
public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery,List<UserRolesResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
    }

    private readonly IApplicationDbContext _context;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public async Task<List<UserRolesResponse>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
      
       
        if (request.UserName != null)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.UserName.Contains(request.UserName), cancellationToken: cancellationToken);
            _ = user ?? throw new NotFoundException(nameof(UserRolesResponse), request.UserName);

            var userRoles = await _userManager.GetRolesAsync(user);

            if (request.Role != null)
            {
                userRoles = userRoles.Where(s => s.Contains(request.Role)).ToList();
            }

            return (await Task.WhenAll(userRoles.Select(async s =>
                    new UserRolesResponse
                    {
                        RoleName = s,
                        UserName = user.UserName,
                        Slug = $"{user.UserName}-{(await _roleManager.FindByNameAsync(s)).Slug}"
                    }))).ToList();
        }
        if (request.Role != null)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(s => s.NormalizedName.Contains(request.Role));
            _ = role ?? throw new NotFoundException(nameof(UserRolesResponse), request.Role);

            var userRoles = await _context.UserRoles.Where(s => s.RoleId == role.Id).ToListAsync();
            var responses = new List<UserRolesResponse>();
            foreach (var userRole in userRoles)
            {
                var response = new UserRolesResponse()
                {
                    RoleName = role.Name,
                    UserName = (await _userManager.FindByIdAsync(userRole.UserId.ToString())).UserName,
                    Slug = $"{(await _userManager.FindByIdAsync(userRole.UserId.ToString())).UserName}-{role.Slug}"
                    
                };
                responses.Add(response);
            }
            return responses;
        }
        var users = await _userManager.Users.ToListAsync();


        var res =await _context.UserRoles.ToListAsync();


        return res.Select(s => new UserRolesResponse
        {
            UserName = (_userManager.Users.FirstOrDefault(u => u.Id == s.UserId)).UserName,
            RoleName = (_roleManager.Roles.FirstOrDefault(r => r.Id == s.RoleId)).Name,
            Slug = s.Slug
        }).ToList();

    }
}
