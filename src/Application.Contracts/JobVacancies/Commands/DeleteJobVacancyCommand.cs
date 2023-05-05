using MediatR;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class DeleteJobVacancyCommand : IRequest<bool>
{
    public long Id { get; set; }
}


