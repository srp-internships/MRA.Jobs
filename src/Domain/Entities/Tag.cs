using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class Tag : BaseEntity
{
    public string Name { get; set; }
    public ICollection<VacancyTag> VacancyTags { get; set; }
    public ICollection<UserTag> UserTags { get; set; }
}


