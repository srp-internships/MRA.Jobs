using MediatR;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Contracts.Internships.Queries;
public class GetInternshipByIdQuery : IRequest<InternshipDetailsDTO>
{
    public Guid Id { get; set; }
}
