﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.Contracts.TimeLineDTO;
public class TimeLineDetailsDto
{
    public Guid UserId { get; set; }
    public Guid CreateBy { get; set; }
    public string UserFullName { get; set; }
    public string UserAvatar { get; set; }
    public string Note { get; set; }
    public DateTime Time { get; set; }
    public TimelineEventType EventType { get; set; }
}
