using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Experiences.Queries;
using MRA.Identity.Application.Contract.Experiences.Responses;

namespace MRA.Identity.Application.Features.Experiences.Queries;
public class GetAllExperienceQueryHandler : IRequestHandler<GetAllExperienceQuery, List<UserExperienceResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllExperienceQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<UserExperienceResponse>> Handle(GetAllExperienceQuery request, CancellationToken cancellationToken)
    {
        var experiences = await _dbContext.Experiences
             .ToListAsync();
        var response = experiences.
            Select(e => _mapper.Map<UserExperienceResponse>(e)).ToList();
        return response;
    }
}
