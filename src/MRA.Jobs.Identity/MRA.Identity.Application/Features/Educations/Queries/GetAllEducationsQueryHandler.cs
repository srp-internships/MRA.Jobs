using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Educations.Responses;

namespace MRA.Identity.Application.Features.Educations.Queries;
public class GetAllEducationsQueryHandler : IRequestHandler<GetAllEducationsQuery, List<UserEducationResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllEducationsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<UserEducationResponse>> Handle(GetAllEducationsQuery request, CancellationToken cancellationToken)
    {
        var educations = await _dbContext.Educations
            .ToListAsync();
        var response = educations.
            Select(e => _mapper.Map<UserEducationResponse>(e)).ToList();
        return response; 
    }
}
