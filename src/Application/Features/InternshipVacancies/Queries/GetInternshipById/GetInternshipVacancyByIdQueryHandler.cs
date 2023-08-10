using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;

public class
    GetInternshipVacancyByIdQueryHandler : IRequestHandler<GetInternshipVacancyByIdQuery, InternshipVacancyResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInternshipVacancyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InternshipVacancyResponse> Handle(GetInternshipVacancyByIdQuery request,
        CancellationToken cancellationToken)
    {
        // var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken);
        InternshipVacancy internship = await _context.Internships
            .Include(i => i.History)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Id == request.Id);
        _ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Id);
        return _mapper.Map<InternshipVacancyResponse>(internship);
    }
}