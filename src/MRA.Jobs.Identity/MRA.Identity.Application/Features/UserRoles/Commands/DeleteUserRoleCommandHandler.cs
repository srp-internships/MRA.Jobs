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
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserRoles.Commands;
public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, ApplicationResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ApplicationResponse<bool>> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Slug == request.Slug);
        if (userRole == null)
            return new ApplicationResponseBuilder<bool>().SetErrorMessage("not found").Success(false).Build();

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
        return new ApplicationResponseBuilder<bool>().Success(true).Build();
    }
}

