using MediatR;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;
public class GetJobVacanciesQuery : IRequest<List<JobVacancyResponse>>
{
}