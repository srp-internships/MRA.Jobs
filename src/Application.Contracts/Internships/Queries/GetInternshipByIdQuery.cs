using MediatR;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Contracts.Internships.Queries;
public class GetInternshipByIdQuery : IRequest<GetInternshipByIdResponse>
{
    public long Id { get; set; }
}
