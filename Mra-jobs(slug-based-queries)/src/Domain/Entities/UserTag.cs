namespace MRA.Jobs.Domain.Entities;

public class UserTag : BaseEntity
{
    public Tag Tag { get; set; }
    public Guid TagId { get; set; }

    public User User { get; set; }
    public Guid UserId { get; set; }
}
