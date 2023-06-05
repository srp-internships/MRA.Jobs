using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;

public class GetJobVacancyByTitleQuery: IRequest<JobVacancyFindResultDto>
{
    public string Title { get; set; }
}