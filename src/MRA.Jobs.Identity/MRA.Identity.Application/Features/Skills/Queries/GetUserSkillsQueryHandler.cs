using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Skills.Queries;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Skills.Queries;
public class GetUserSkillsQueryHandler : IRequestHandler<GetUserSkillsQuery, ApplicationResponse<UserSkillsResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public GetUserSkillsQueryHandler(IApplicationDbContext context, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<ApplicationResponse<UserSkillsResponse>> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var roles = _userHttpContextAccessor.GetUserRoles();
            var userName = _userHttpContextAccessor.GetUserName();
            if (request.UserName != null && roles.Any(role => role == "Applicant") && userName != request.UserName)
                return new ApplicationResponseBuilder<UserSkillsResponse>()
                    .SetErrorMessage("Access is denied")
                    .Success(false).Build();

           if (request.UserName!= null)
                userName= request.UserName;

            var user = await _context.Users
                .Include(u => u.UserSkills)
                .ThenInclude(us=>us.Skill)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return new ApplicationResponseBuilder<UserSkillsResponse>()
                    .SetErrorMessage("User not found")
                    .Success(false).Build();

            var response = new UserSkillsResponse
            {
                Skills = user.UserSkills.Select(us => us.Skill.Name).ToList()
            };                  

            return new ApplicationResponseBuilder<UserSkillsResponse>().SetResponse(response).Build();
        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<UserSkillsResponse>().SetException(ex).Success(false).Build();
        }
    }
}

