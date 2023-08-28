using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.UserRoles.Queries;
using MRA.Identity.Application.Contract.UserRoles.Response;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Queries;
public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, ApplicationResponse<List<UserRolesResponse>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    public async Task<ApplicationResponse<List<UserRolesResponse>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
       return null;

    }
}
