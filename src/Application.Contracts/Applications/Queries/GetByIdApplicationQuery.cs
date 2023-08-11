using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;

public class GetByIdApplicationQuery : IRequest<ApplicationDetailsDto>
{
    public Guid Id { get; set; }
}