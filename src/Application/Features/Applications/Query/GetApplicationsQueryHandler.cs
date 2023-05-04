using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;

namespace MRA.Jobs.Application.Features.Applications.Query;

using AutoMapper;
using MRA.Jobs.Domain.Entities;


public record GetApplicationsQuery : IRequest<IEnumerable<Application>>;
public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, IEnumerable<Application>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetApplicationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Application>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications = await _context.Applications.ToListAsync();
        return _mapper.Map<List<Application>>(applications);
    }
}
