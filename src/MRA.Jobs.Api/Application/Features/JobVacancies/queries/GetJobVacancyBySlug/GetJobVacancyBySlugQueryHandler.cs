using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyBySlug;

public class GetJobVacancyBySlugQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper,
    ICurrentUserService currentUser)
    : IRequestHandler<GetJobVacancyBySlugQuery, JobVacancyDetailsDto>
{
    public async Task<JobVacancyDetailsDto> Handle(GetJobVacancyBySlugQuery request,
        CancellationToken cancellationToken)
    {
        JobVacancy jobVacancy = await dbContext.JobVacancies
            .Include(i => i.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .Include(i => i.History)
            .Include(i => i.Tags).ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken: cancellationToken);
        _ = jobVacancy ?? throw new NotFoundException(nameof(JobVacancy), request.Slug);

        jobVacancy.History =
            await dbContext.VacancyTimelineEvents.Where(t => t.VacancyId == jobVacancy.Id)
                .ToListAsync(cancellationToken: cancellationToken);
        
        var mapped = mapper.Map<JobVacancyDetailsDto>(jobVacancy);
        mapped.IsApplied = await dbContext.Applications.AnyAsync(
            s => s.ApplicantUsername == currentUser.GetUserName() && s.VacancyId == jobVacancy.Id,
            cancellationToken: cancellationToken);
        return mapped;
    }
}