using MediatR;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Responses;
public class JobVacancyResponse : IRequest<JobVacancyResponse>
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
}