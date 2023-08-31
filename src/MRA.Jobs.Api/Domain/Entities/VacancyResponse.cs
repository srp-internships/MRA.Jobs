using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class VacancyResponse
{
    public Guid Id { get; set; }
    public string Response { get; set; }
    public Guid ApplicationId { get; set; }
    public VacancyQuestion Question { get; set; }
}
