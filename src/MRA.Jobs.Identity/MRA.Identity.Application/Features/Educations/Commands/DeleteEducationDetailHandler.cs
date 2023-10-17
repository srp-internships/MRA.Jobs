using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Educations.Command.Delete;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class DeleteEducationDetailHandler : IRequestHandler<DeleteEducationCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public DeleteEducationDetailHandler(IApplicationDbContext context, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<bool> Handle(DeleteEducationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _userHttpContextAccessor.GetUserId();
        var user = await _context.Users
            .Include(u => u.Educations)
            .FirstOrDefaultAsync(u => u.Id == userId);
        _ = user ?? throw new NotFoundException("user is not found");

        var education = user.Educations.FirstOrDefault(e => e.Id == request.Id);
        _ = education ?? throw new NotFoundException("This User has not such education");

        user.Educations.Remove(education);

        await _context.SaveChangesAsync();

        return true;
    }
}