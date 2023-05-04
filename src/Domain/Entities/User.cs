namespace MRA.Jobs.Domain.Entities;

public class User : BaseEntity
{
    public string Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<UserTimelineEvent> userTimelineEvents { get; set; }
    public ICollection<UserTag> UserTags { get; set; }  
}