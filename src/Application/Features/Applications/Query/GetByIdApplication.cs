using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications.Query;
public class GetByIdApplication : IRequestHandler<GetByIdApplicationQuery, ApplicationResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetByIdApplication(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApplicationResponse> Handle(GetByIdApplicationQuery request, CancellationToken cancellationToken)
    {
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == request.Id);

        return _mapper.Map<ApplicationResponse>(application);
    }
}
