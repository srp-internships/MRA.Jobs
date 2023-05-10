using MRA.Jobs.Application.Contracts.Internships.Queries;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.Internships.Queries.GetInternshipById;
public class GetInternshipByIdQueryHandler : IRequestHandler<GetInternshipByIdQuery, InternshipDetailsDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInternshipByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<InternshipDetailsDTO> Handle(GetInternshipByIdQuery request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken);
        _ = internship ?? throw new NotFoundException(nameof(Internship), request.Id);
        return _mapper.Map<InternshipDetailsDTO>(internship);
    }
}
