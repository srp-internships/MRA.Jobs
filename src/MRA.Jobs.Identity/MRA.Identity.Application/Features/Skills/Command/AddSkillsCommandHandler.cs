using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Skills.Command;
public class AddSkillsCommandHandler : IRequestHandler<AddSkillsCommand, UserSkillsResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public AddSkillsCommandHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;

    }
    public async Task<UserSkillsResponse> Handle(AddSkillsCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(u => u.UserSkills)
            .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.UserName.Equals(_userHttpContextAccessor.GetUserName()),
            cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");


        foreach (var skillName in request.Skills)
        {
            var existingSkill = await _context.Skills.FirstOrDefaultAsync(s => s.Name == skillName, cancellationToken);

            if (existingSkill == null)
            {
                existingSkill = new Skill { Name = skillName };
                _context.Skills.Add(existingSkill);
            }

            if (!user.UserSkills.Any(us => us.SkillId == existingSkill.Id))
            {
                user.UserSkills.Add(new UserSkill { Skill = existingSkill });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var response = new UserSkillsResponse
        {
            Skills = user.UserSkills.Select(us => us.Skill.Name).ToList()
        };

        return response;
    }

}

