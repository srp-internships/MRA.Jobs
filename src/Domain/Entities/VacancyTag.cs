using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class VacancyTag: BaseEntity
{
    public Tag Tag { get; set; }
    public long TagId { get; set; }

    public Vacancy Vacancy { get; set; }
    public long VacancyId { get; set; }   
}
