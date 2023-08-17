using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class Skill
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string SkillName { get; set; }
}
