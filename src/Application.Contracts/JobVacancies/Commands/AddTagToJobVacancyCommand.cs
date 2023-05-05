using MediatR;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class AddTagToJobVacancyCommand : IRequest<bool>
{
    public long JobVacancyId { get; set; }
    public long TagId { get; set; }
}


