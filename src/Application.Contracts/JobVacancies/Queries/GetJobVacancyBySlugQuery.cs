using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;

public class GetJobVacancyBySlugQuery : IRequest<JobVacancyDetailsDto>
{
    public string Slug { get; set; }
}


