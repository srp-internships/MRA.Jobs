using MediatR;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class CreateJobVacancyCommand : IRequest<long>
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public long CategoryId { get; set; }
    public int RequiredYearOfExperience { get; set; }
    public WorkSchedule WorkSchedule { get; set; }
}


