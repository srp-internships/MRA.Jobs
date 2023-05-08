using MediatR;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Contracts.Internships.Queries;
public class GetInternshipsQuery : IRequest<List<InternshipListDTO>>
{
}
