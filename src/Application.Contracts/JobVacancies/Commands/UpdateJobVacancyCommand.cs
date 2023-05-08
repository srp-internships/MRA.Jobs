using MediatR;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class UpdateJobVacancyCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public int RequiredYearOfExperience { get; set; }
    public WorkSchedule WorkSchedule { get; set; }
}


