using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;
public class CreateJobVacancyTestCommand : IRequest<TestInfoDTO>
{
    public Guid Id { get; set; }
    public long NumberOfQuestion { get; set; }
    public List<string> Categories { get; set; }
}
