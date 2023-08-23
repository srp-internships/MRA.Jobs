using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class JobQuestion
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Response { get; set; }
}
