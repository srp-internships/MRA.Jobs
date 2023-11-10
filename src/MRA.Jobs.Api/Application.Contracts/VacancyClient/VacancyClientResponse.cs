using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.Contracts.VacancyClient;
public class VacancyClientResponse
{
    public string Category { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public DateTime? PublishDate { get; set; }
    public int Duration { get; set; } = 0;
    public DateTime? Deadline { get; set; }
    public DateTime? EndDate { get; set; }
    public string Slug { get; set; }
}
