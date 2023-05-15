using MediatR;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;
public class GetByIdApplicationQuery : IRequest<ApplicationDetailsDTO>
{
    public Guid Id { get; set; }
}
