using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Enums;
public enum TimelineEventType
{
    Created,
    Updated,
    Deleted,
    StatusChanged,
    Note,
    Error
}
