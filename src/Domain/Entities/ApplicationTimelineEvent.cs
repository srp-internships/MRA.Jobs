namespace MRA.Jobs.Domain.Entities;

public class ApplicationTimelineEvent : TimelineEvent
{
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
}
