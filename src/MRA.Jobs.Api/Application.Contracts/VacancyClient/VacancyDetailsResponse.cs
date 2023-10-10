using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.Contracts.VacancyClient;
public class VacancyDetailsResponse
{
    public string Title { get; set; }
    public int Duration { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
}
