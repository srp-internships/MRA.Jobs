using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA_Jobs.Domain.Entities;
public class Applicant
{
    public int Id { get; set; }
    public User User { get; set; }
}
