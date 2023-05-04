using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class Test : BaseAuditableEntity
{ 
    public string Description { get; set; }
    public string Duration { get; set; }
    public long NumberOfQuestion { get; set; }
    public string Title { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long VacancyId { get; set; }
    public int PassingScore { get; set; }
    public string TestResults { get; set; }
    public Vacancy Vacancy { get; set; }

}
