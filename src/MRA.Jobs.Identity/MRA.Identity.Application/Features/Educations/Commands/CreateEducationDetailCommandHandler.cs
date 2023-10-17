using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class
    CreateEducationDetailCommandHandler : IRequestHandler<CreateEducationDetailCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public CreateEducationDetailCommandHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
    }
    public async Task<Guid> Handle(CreateEducationDetailCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _userHttpContextAccessor.GetUserId();
        var user = await _context.Users
            .Include(u => u.Educations)
            .FirstOrDefaultAsync(u => u.Id.Equals(userId));
        _ = user ?? throw new NotFoundException("user is not found");

        var education = new EducationDetail()
        {
            University = request.University,
            Speciality = request.Speciality,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            UntilNow = request.UntilNow
        };
        user.Educations.Add(education);
        await _context.SaveChangesAsync();
        return education.Id;
    }
}