using MediatR;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;
public class GetJobVacancyByIdQuery : IRequest<Responses.JobVacancyFullResponse>
{
    public long Id { get; set; }
}
