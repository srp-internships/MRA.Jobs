using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.TimelineEvents.Responses;
public class GetTimelineEventsResponse
{
    public long Id { get; set; }
    public int CreateByUserId { get; set; }
    public string Note { get; set; }
    public DateTime Time { get; set; }
    public TimelineEventType EventType { get; set; }
}
