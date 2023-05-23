using MediatR;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class AddTagsToJobVacancyCommand : IRequest<bool>
{
    public Guid JobVacancyId { get; set; }
    public string[] Tags { get; set; }
}


