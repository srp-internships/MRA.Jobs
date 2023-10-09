using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Command.Delete;
using MRA.Identity.Application.Contract.Experience.Command.Delete;

namespace MRA.Identity.Application.Features.Experiences.Commands;

public class DeleteExperienceDetailHandler : IRequestHandler<DeleteExperienceCommand, ApplicationResponse<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public DeleteExperienceDetailHandler(IApplicationDbContext context, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<ApplicationResponse<bool>> Handle(DeleteExperienceCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContextAccessor.GetUserId();
            var user = await _context.Users
                .Include(u => u.Experiences)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new ApplicationResponseBuilder<bool>().SetErrorMessage("User not found").Success(false).Build();

            var experience = user.Experiences.FirstOrDefault(e => e.Id == request.Id);
            if (experience == null)
                return new ApplicationResponseBuilder<bool>().SetErrorMessage("This User has not such experience")
                    .Success(false).Build();

            user.Experiences.Remove(experience);

            await _context.SaveChangesAsync();

            return new ApplicationResponseBuilder<bool>().SetResponse(true).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<bool>().SetException(e).Success(false).Build();
        }
    }
}