using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Experiences.Query;
using MRA.Identity.Application.Contract.Experiences.Responses;

namespace MRA.Identity.Application.Features.Educations.Queries;

public class GetAllExperiencesByUserQueryHandler(
    IApplicationDbContext context,
    IUserHttpContextAccessor userHttpContextAccessor,
    IMapper mapper)
    : IRequestHandler<GetExperiencesByUserQuery, List<UserExperienceResponse>>
{
    public async Task<List<UserExperienceResponse>> Handle(GetExperiencesByUserQuery request,
        CancellationToken cancellationToken)
    {
        var roles = userHttpContextAccessor.GetUserRoles();
        var userName = userHttpContextAccessor.GetUserName();
        if (request.UserName != null && !roles.Any())
            throw new ForbiddenAccessException("Access is denied");

        if (request.UserName != null)
            userName = request.UserName;

        var user = await context.Users
            .Include(u => u.Experiences)
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken: cancellationToken);

        _ = user ?? throw new NotFoundException("user is not found");
        var userExperiencesResponses = user.Experiences
            .Select(mapper.Map<UserExperienceResponse>).ToList();
        return userExperiencesResponses;
    }
}
