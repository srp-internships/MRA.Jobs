using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationById;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Domain.Entities;
public class GetApplicationByIdQueryHandler : IRequestHandler<GetByIdApplicationQuery, ApplicationDetailsDTO>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetApplicationByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApplicationDetailsDTO> Handle(GetByIdApplicationQuery request, CancellationToken cancellationToken)
    {
       // var application = await _dbContext.Applications.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
      var application = await _dbContext.Applicants
            .Include(a=>a.History)
            .FirstOrDefaultAsync(a=>a.Id == request.Id, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Id);

        return _mapper.Map<ApplicationDetailsDTO>(application);
    }
}
