using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Skills.Queries;
using MRA.Identity.Application.Contract.Skills.Responses;

namespace MRA.Identity.Application.Features.Skills.Queries;
public class GetAllSkillsQueryHandler : IRequestHandler<GetAllSkillsQuery, UserSkillsResponse>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllSkillsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<UserSkillsResponse> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        var skills = await _dbContext.Skills.ToListAsync();
        return new UserSkillsResponse()
        {
            Skills = skills.Select(s => s.Name).ToList(),
        };
    }
}
