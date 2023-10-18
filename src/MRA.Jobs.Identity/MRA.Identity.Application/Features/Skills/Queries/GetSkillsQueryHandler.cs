using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Skills.Queries;

namespace MRA.Identity.Application.Features.Skills.Queries;
public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, List<String>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetSkillsQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<string>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        var Skills = await _applicationDbContext.Skills
            .Select(s => s.Name).ToListAsync(cancellationToken);

        return Skills;
    }
}
