namespace MRA.Identity.Domain.Entities;
public class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<UserSkill> UserSkills { get; set; }
}
