using MRA.Jobs.Application.Contracts.Internships.Queries;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;
public class GetInternshipVacancyByIdQueryHandler : IRequestHandler<GetInternshipVacancyByIdQuery, InternshipVacancyResponce>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInternshipVacancyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<InternshipVacancyResponce> Handle(GetInternshipVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken);
        _ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Id);
        return _mapper.Map<InternshipVacancyResponce>(internship);
    }
}
