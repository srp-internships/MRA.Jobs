using MediatR;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;

public class GetJobVacancyByIdQuery : IRequest<JobVacancyDetailsDTO>
{
    public long Id { get; set; }
}


