﻿namespace MRA.Jobs.Domain.Entities;

public abstract class TimelineEvent : BaseEntity
{
    public string CreateBy { get; set; }
    public string Note { get; set; }
    public DateTime Time { get; set; }
    public TimelineEventType EventType { get; set; }
}