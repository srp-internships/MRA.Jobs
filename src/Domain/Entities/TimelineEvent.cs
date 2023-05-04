using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public abstract class TimelineEvent : BaseEntity
{
    public int CreateByUserId { get; set; }
    public string Note { get; set; }
    public DateTime Time { get; set; }
    public TimelineEventType EventType { get; set; }
}
