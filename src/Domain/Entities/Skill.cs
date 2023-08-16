using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class Skill
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public string SkillName { get; set; }
}
