using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Skills.Queries;
using MRA.Identity.Application.Contract.Skills.Responses;

namespace MRA.Identity.Application.Features.Skills.Queries;
public class GetUserSkillsQueryHandler(IApplicationDbContext context, IUserHttpContextAccessor userHttpContextAccessor)
    : IRequestHandler<GetUserSkillsQuery, UserSkillsResponse>
{
    public async Task<UserSkillsResponse> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
    {
        var roles = userHttpContextAccessor.GetUserRoles();
        var userName = userHttpContextAccessor.GetUserName();
        if (request.UserName != null && !roles.Any())
            throw new ForbiddenAccessException("Access is denied");
        if (request.UserName != null)
            userName = request.UserName;

        var user = await context.Users
            .Include(u => u.UserSkills)
            .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");

        var response = new UserSkillsResponse
        {
            Skills = user.UserSkills.Select(us => us.Skill.Name).ToList()
        };

        return response;
    }
}

