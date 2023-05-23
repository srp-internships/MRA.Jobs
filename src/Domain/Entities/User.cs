namespace MRA.Jobs.Domain.Entities;

public abstract class User : BaseAuditableEntity
{
    public string Avatar { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public ICollection<UserTimelineEvent> History { get; set; }
    public IList<UserTag> Tags { get; set; }
}
