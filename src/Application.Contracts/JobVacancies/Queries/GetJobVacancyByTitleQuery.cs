using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;

public class GetJobVacancyByTitleQuery: IRequest<JobVacancyListDTO>
{
    public string Title { get; set; }
}