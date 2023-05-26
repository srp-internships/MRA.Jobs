using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;
public class CreateJobVacancyTestCommand : IRequest<TestInfoDTO>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TimeSpan Duration { get; set; }
    public long NumberOfQuestion { get; set; }
    public int PassingScore { get; set; }
    public Guid VacancyId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }
    public List<string> Categories { get; set; }
}
