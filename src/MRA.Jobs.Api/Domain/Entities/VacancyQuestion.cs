using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class VacancyQuestion
{
    public Guid Id { get; set; }
    public string Question { get; set; }
}
