using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA_Jobs.Domain.Entities;
public class Category:BaseEntity
{
    public string Name { get; set; }
    public ICollection<JobVacancy>? JobVacancies { get; set; }
    public ICollection<EducationVacancy>? EducationVacancies { get; set; }
}
