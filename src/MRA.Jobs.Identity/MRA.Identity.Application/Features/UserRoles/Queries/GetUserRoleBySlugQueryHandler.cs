using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Queries;
using MRA.Identity.Application.Contract.UserRoles.Response;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Queries;
public class GetUserRoleBySlugQueryHandler : IRequestHandler<GetUserRolesBySlugQuery, ApplicationResponse<UserRolesResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public GetUserRoleBySlugQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
    }

  

    public Task<ApplicationResponse<UserRolesResponse>> Handle(GetUserRolesBySlugQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
