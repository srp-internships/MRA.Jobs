using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class
    UpdateEducationDetailCommandHandler : IRequestHandler<UpdateEducationDetailCommand, Guid>
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

    public async Task<Guid> Handle(UpdateEducationDetailCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _userHttpContextAccessor.GetUserId();
        var user = await _context.Users
            .Include(u => u.Educations)
            .FirstOrDefaultAsync(u => u.Id.Equals(userId));
        _ = user ?? throw new NotFoundException("user is not found");
        var education = user.Educations.FirstOrDefault(e => e.Id.Equals(request.Id));
        _ = education ?? throw new NotFoundException("education not exits");

        _mapper.Map(request, education);
        await _context.SaveChangesAsync();
        return education.Id;
    }
}