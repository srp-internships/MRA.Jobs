using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.UserRoles.Commands;

namespace MRA.Identity.Application.Features.UserRoles.Commands;

public class DeleteUserRoleCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteUserRoleCommand, bool>
{
    public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole =
            await context.UserRoles.FirstOrDefaultAsync(ur => ur.Slug == request.Slug,
                cancellationToken);
        
        _ = userRole ?? throw new NotFoundException("not found");
        context.UserRoles.Remove(userRole);
        
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}