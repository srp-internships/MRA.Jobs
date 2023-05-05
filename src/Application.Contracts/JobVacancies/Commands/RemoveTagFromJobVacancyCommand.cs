using MediatR;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class RemoveTagFromJobVacancyCommand : IRequest<bool>
{
    public Guid JobVacancyId { get; set; }
    public Guid TagId { get; set; }
}


