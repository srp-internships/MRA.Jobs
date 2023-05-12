using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationById;
public class GetApplicationByIdQueryHandler : IRequestHandler<GetByIdApplicationQuery, ApplicationListDTO>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetApplicationByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApplicationListDTO> Handle(GetByIdApplicationQuery request, CancellationToken cancellationToken)
    {
        var application = await _dbContext.Applications.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(JobVacancy), request.Id);

        return _mapper.Map<ApplicationListDTO>(application);
    }
}
