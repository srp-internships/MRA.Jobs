using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class UserTimelineEvent : TimelineEvent
{
    public long UserId { get; set; }
    public User User { get; set; }
}
