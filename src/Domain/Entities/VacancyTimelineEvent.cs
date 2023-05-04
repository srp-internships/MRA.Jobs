using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class VacancyTimelineEvent : TimelineEvent
{
    public long VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
}
