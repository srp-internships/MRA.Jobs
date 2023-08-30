namespace MRA.Jobs.Domain.Entities;

public class UserTimelineEvent : TimelineEvent
{
    public Guid UserId { get; set; }
}