using FluentValidation;

namespace MRA.Identity.Application.Contract.Skills.Command;
public class RemoveUserSkillCommandValidaTor : AbstractValidator<RemoveUserSkillCommand>
{
    public RemoveUserSkillCommandValidaTor()
    {
        RuleFor(us => us.Skill).NotEmpty();
    }
}
