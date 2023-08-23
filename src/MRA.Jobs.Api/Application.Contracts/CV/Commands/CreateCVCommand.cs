using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.CV.Commands;
public record CreateCVCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public IEnumerable<EducationDetail> EducationDetails { get; init; }
    public IEnumerable<ExperienceDetail> ExperienceDetails { get; init; }
    public IEnumerable<Skill> Skills { get; init; }
}
