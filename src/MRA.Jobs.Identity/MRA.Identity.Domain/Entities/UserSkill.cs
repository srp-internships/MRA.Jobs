namespace MRA.Identity.Domain.Entities;
public class UserSkill
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid SkillId { get; set; }
    public Skill Skill { get; set; }
}