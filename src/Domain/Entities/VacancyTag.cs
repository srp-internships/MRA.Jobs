using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class VacancyTag: BaseEntity
{
    public Tag Tag { get; set; }
    public int TagId { get; set; }

    public Vacancy Vacancy { get; set; }
    public int VacancyId { get; set; }   
}
