namespace MRA.Identity.Domain.Entities;
public class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
}
