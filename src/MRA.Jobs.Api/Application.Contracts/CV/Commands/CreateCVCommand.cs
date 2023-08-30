using System;
using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Contracts.CV.Commands;
public record CreateCVCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public IEnumerable<EducationDetailDto> EducationDetails { get; init; }
    public IEnumerable<ExperienceDetailDto> ExperienceDetails { get; init; }
    public IEnumerable<SkillDto> Skills { get; init; }
}
