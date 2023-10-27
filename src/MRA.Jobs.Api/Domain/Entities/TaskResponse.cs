using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class TaskResponse
{
    public Guid Id { get; set; }
    public Guid TaksId { get; set; }
    public string Code { get; set; }
}
