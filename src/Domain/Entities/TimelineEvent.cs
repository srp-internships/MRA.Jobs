using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class TimelineEvent:BaseEntity
{
    public int CreateByUserId { get; set; }
    public string Note { get; set; }
    public DateTime Time { get; set; }
}
