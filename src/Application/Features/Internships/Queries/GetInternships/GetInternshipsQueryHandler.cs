using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Internships.Queries;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.Internships.Queries.GetInternships;
public class GetInternshipsQueryHandler : IRequestHandler<GetInternshipsQuery, List<InternshipListDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInternshipsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<InternshipListDTO>> Handle(GetInternshipsQuery request, CancellationToken cancellationToken)
    {
        var internships = await _context.Internships
            .ToListAsync();

        return _mapper.Map<List<InternshipListDTO>>(internships);
    }
}
