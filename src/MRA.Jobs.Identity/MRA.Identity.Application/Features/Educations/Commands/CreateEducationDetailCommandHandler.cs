using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class
    CreateEducationDetailCommandHandler : IRequestHandler<CreateEducationDetailCommand, ApplicationResponse<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public CreateEducationDetailCommandHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
    }


    public async Task<ApplicationResponse<Guid>> Handle(CreateEducationDetailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContextAccessor.GetUserId();
            var user = await _context.Users
                .Include(u => u.Educations)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));
            if (user == null)
                return new ApplicationResponseBuilder<Guid>()
                    .SetErrorMessage("User not found")
                    .Success(false).Build();
            var education = new EducationDetail()
            {
                University = request.University,
                Speciality = request.Speciality,
                StartDate = request.StartDate.HasValue ? request.StartDate.Value : default(DateTime),
                EndDate = request.EndDate.HasValue ? request.EndDate.Value : default(DateTime),
                UntilNow = request.UntilNow
            };

            user.Educations.Add(education);

            await _context.SaveChangesAsync();

            return new ApplicationResponseBuilder<Guid>().SetResponse(education.Id).Build();
        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<Guid>().SetException(ex).Success(false).Build();
        }
    }
}