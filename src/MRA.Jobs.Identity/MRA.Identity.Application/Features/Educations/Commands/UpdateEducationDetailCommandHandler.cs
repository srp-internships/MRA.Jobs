using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class
    UpdateEducationDetailCommandHandler : IRequestHandler<UpdateEducationDetailCommand, ApplicationResponse<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;
    private readonly IMapper _mapper;

    public UpdateEducationDetailCommandHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor,
        IMapper mapper)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ApplicationResponse<Guid>> Handle(UpdateEducationDetailCommand request,
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
            var education = user.Educations.FirstOrDefault(e => e.Id.Equals(request.Id));

            if (education == null)
                return new ApplicationResponseBuilder<Guid>().SetErrorMessage("education not exits")
                    .Success(false).Build();

            _mapper.Map(request, education);
            await _context.SaveChangesAsync();
            return new ApplicationResponseBuilder<Guid>().SetResponse(education.Id).Build();
        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<Guid>().SetException(ex).Success(false).Build();
        }
    }
}