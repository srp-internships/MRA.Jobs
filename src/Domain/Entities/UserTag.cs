using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class UserTag :BaseEntity
{
    public Tag Tag { get; set; }
    public int TagId { get; set; }
    public int UserId { get; set; }
}
