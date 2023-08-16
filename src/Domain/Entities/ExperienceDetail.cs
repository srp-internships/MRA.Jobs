using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class ExperienceDetail
{
    public Guid UserId { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string JobLocationCity { get; set; }
    public string JobLocationCountry { get; set; }
    public string Description { get; set; }
}
