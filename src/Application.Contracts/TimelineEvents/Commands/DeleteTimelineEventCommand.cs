using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.TimelineEvents.Commands;
public class DeleteTimelineEventCommand:IRequest<bool>
{
    public long Id { get; set; }
}
