using MediatR;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;
public class GetByIdApplicationQuery : IRequest<ApplicationListDTO>
{
    public Guid Id { get; set; }
}
