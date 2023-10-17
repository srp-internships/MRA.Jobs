using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Profile.Queries;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Identity.Application.Features.UserProfiles.Query;
public class GetProfileQueryHandler : IRequestHandler<GetPofileQuery, UserProfileResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public GetProfileQueryHandler(IApplicationDbContext context,
        IMapper mapper, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<UserProfileResponse> Handle(GetPofileQuery request, CancellationToken cancellationToken)
    {
        var userRoles = _userHttpContextAccessor.GetUserRoles();
        var userName = _userHttpContextAccessor.GetUserName();

        if (request.UserName != null && userRoles.Any(role => role == "Applicant") && userName != request.UserName)
            throw new ValidationException("Access is denied");

        if (request.UserName != null)
            userName = request.UserName;

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
        _ = user ?? throw new NotFoundException("user is not found");
        var response = _mapper.Map<UserProfileResponse>(user);
        return response;
    }
}
