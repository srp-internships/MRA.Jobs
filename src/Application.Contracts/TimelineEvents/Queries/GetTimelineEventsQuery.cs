using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Application.Contracts.TimelineEvents.Responses;

namespace MRA.Jobs.Application.Contracts.TimelineEvents.Queries;
public class GetTimelineEventsQuery:IRequest<List<GetTimelineEventsResponse>>
{

}
