using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Command.Delete;
using MRA.Identity.Application.Contract.Experience.Command.Delete;

namespace MRA.Identity.Application.Features.Experiences.Commands;

public class DeleteExperienceDetailHandler : IRequestHandler<DeleteExperienceCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public DeleteExperienceDetailHandler(IApplicationDbContext context, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<bool> Handle(DeleteExperienceCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _userHttpContextAccessor.GetUserId();
        var user = await _context.Users
            .Include(u => u.Experiences)
            .FirstOrDefaultAsync(u => u.Id == userId);
        _ = user ?? throw new NotFoundException("user is not found");

        var experience = user.Experiences.FirstOrDefault(e => e.Id == request.Id);
        _ = experience ?? throw new NotFoundException("This User has not such experience");

        user.Experiences.Remove(experience);

        await _context.SaveChangesAsync();

        return true;
    }
}