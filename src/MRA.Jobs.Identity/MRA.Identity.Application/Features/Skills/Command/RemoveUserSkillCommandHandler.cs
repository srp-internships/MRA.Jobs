using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Skills.Command;
public class RemoveUserSkillCommandHandler : IRequestHandler<RemoveUserSkillCommand, ApplicationResponse<UserSkillsResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public RemoveUserSkillCommandHandler(IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _userHttpContextAccessor = userHttpContextAccessor;
    }
    public async Task<ApplicationResponse<UserSkillsResponse>> Handle(RemoveUserSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userName = _userHttpContextAccessor.GetUserName();
            var user = await _context.Users.Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.UserName.Equals(_userHttpContextAccessor.GetUserName()),
                cancellationToken);

            if (user == null)
                return new ApplicationResponseBuilder<UserSkillsResponse>()
                .SetErrorMessage("User not found")
                .Success(false).Build();


            var specificSkill = user.UserSkills
                .FirstOrDefault(us => us.Skill.Name == request.Skill);

            if (specificSkill == null)
                return new ApplicationResponseBuilder<UserSkillsResponse>()
                    .SetErrorMessage($"Skill '{request.Skill}' not found for this user")
                    .Success(false).Build();

            user.UserSkills.Remove(specificSkill);

            await _context.SaveChangesAsync();

            var response = new UserSkillsResponse
            {
                Skills = user.UserSkills.Select(us => us.Skill.Name).ToList()
            };

            return new ApplicationResponseBuilder<UserSkillsResponse>()
                .SetResponse(response).Build();
        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<UserSkillsResponse>().SetException(ex)
                .SetErrorMessage(ex.Message.ToString())
                .Success(false).Build();
        }
    }
}
