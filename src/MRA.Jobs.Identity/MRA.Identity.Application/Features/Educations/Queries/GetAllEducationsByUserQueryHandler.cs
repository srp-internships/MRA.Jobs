using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Educations.Responses;

namespace MRA.Identity.Application.Features.Educations.Queries;

public class GetAllEducationsByUserQueryHandler(
    IApplicationDbContext context,
    IUserHttpContextAccessor userHttpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GetEducationsByUserQuery, List<UserEducationResponse>>
{
    public async Task<List<UserEducationResponse>> Handle(GetEducationsByUserQuery request,
        CancellationToken cancellationToken)
    {
        var roles = userHttpContextAccessor.GetUserRoles();
        var userName = userHttpContextAccessor.GetUserName();
        if (request.UserName != null && !roles.Any())
            throw new ForbiddenAccessException("Access is denied");

        if (request.UserName != null)
            userName = request.UserName;

        var user = await context.Users
            .Include(u => u.Educations)
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");

        var userEducationResponses = user.Educations
            .Select(mapper.Map<UserEducationResponse>).ToList();

        return userEducationResponses;
    }
}
