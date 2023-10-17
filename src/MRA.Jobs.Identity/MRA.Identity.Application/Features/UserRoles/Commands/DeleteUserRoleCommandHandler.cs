using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.UserRoles.Commands;

namespace MRA.Identity.Application.Features.UserRoles.Commands;
public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Slug == request.Slug);
        _ = userRole ?? throw new NotFoundException("not found");
        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
        return true;
    }
}

