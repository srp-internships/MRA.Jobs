using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Tags;

namespace MRA.Jobs.Application.Features.Tags;

public class GetTagsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTagsQuery,List<string>>
{
    public async Task<List<string>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        return (await dbContext.Tags.Select(t => t.Name).Distinct().ToListAsync(cancellationToken));
    }
}