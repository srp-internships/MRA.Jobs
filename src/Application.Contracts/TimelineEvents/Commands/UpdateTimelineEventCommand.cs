using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.TimelineEvents.Commands;
public class UpdateTimelineEventCommand : IRequest<long>
{
    public long Id { get; set; }
    public int CreateByUserId { get; set; }
    public string Note { get; set; }
    public TimelineEventType EventType { get; set; }
}
