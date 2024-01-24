using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Profile.Queries;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Identity.Application.Features.UserProfiles.Query;
public class GetProfileQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    IUserHttpContextAccessor userHttpContextAccessor)
    : IRequestHandler<GetPofileQuery, UserProfileResponse>
{
    public async Task<UserProfileResponse> Handle(GetPofileQuery request, CancellationToken cancellationToken)
    {
        var userRoles = userHttpContextAccessor.GetUserRoles();
        var userName = userHttpContextAccessor.GetUserName();

        if (request.UserName != null && !userRoles.Any())
            throw new ForbiddenAccessException("Access is denied");

        if (request.UserName != null)
            userName = request.UserName;

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");
        var response = mapper.Map<UserProfileResponse>(user);
        return response;
    }
}
