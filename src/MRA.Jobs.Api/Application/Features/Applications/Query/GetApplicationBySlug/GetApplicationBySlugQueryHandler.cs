using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationBySlug;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationBySlug;
using MRA.Jobs.Domain.Entities;

public class GetApplicationBySlugQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetBySlugApplicationQuery, ApplicationDetailsDto>
{
    public async Task<ApplicationDetailsDto> Handle(GetBySlugApplicationQuery request,
        CancellationToken cancellationToken)
    {
        var application = await dbContext.Applications
            .Include(a => a.History)
            .Include(s => s.VacancyResponses)
            .Include(s => s.TaskResponses)
            .FirstOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug);

        return mapper.Map<ApplicationDetailsDto>(application);
    }
}