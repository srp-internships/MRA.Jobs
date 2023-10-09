using FluentValidation;

namespace MRA.Identity.Application.Contract.Skills.Command;
public class RemoveUserSkillCommandValidator : AbstractValidator<RemoveUserSkillCommand>
{
    public RemoveUserSkillCommandValidator()
    {
        RuleFor(us => us.Skill).NotEmpty();
    }
}
