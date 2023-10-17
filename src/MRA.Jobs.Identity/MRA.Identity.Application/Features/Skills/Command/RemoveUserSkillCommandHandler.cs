using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Skills.Command;
public class RemoveUserSkillCommandHandler : IRequestHandler<RemoveUserSkillCommand, UserSkillsResponse>
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
    public async Task<UserSkillsResponse> Handle(RemoveUserSkillCommand request, CancellationToken cancellationToken)
    {
        var userName = _userHttpContextAccessor.GetUserName();
        var user = await _context.Users.Include(u => u.UserSkills)
            .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.UserName.Equals(_userHttpContextAccessor.GetUserName()),
            cancellationToken);

        _ = user ?? throw new NotFoundException("user is not found");

        var specificSkill = user.UserSkills
            .FirstOrDefault(us => us.Skill.Name == request.Skill);
        _ = specificSkill ?? throw new NotFoundException($"Skill '{request.Skill}' not found for this user");

        user.UserSkills.Remove(specificSkill);

        await _context.SaveChangesAsync();

        var response = new UserSkillsResponse
        {
            Skills = user.UserSkills.Select(us => us.Skill.Name).ToList()
        };

        return response;
    }
       
}
