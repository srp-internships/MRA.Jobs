using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Command.Delete;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class DeleteEducationDetailHandler : IRequestHandler<DeleteEducationCommand, ApplicationResponse<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public DeleteEducationDetailHandler(IApplicationDbContext context, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<ApplicationResponse<bool>> Handle(DeleteEducationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContextAccessor.GetUserId();
            var user = await _context.Users
                .Include(u => u.Educations)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new ApplicationResponseBuilder<bool>().SetErrorMessage("User not found").Success(false).Build();

            var education = user.Educations.FirstOrDefault(e => e.Id == request.Id);
            if (education == null)
                return new ApplicationResponseBuilder<bool>().SetErrorMessage("This User has not such education")
                    .Success(false).Build();

            user.Educations.Remove(education);

            await _context.SaveChangesAsync();

            return new ApplicationResponseBuilder<bool>().SetResponse(true).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<bool>().SetException(e).Success(false).Build();
        }
    }
}