using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Application.Features.Applications.Query;

using AutoMapper;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;

public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, IEnumerable<ApplicationResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetApplicationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApplicationResponse>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications = await _context.Applications
            .Include(a => a.Vacancy)
            .Include(a => a.Applicant)
            .ToListAsync();
        return _mapper.Map<List<ApplicationResponse>>(applications);
    }
}
