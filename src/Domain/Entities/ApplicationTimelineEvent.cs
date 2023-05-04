using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities
{
    public class ApplicationTimelineEvent : TimelineEvent
    {
        public long ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
