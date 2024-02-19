using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobs;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobs;

public class GetJobsQueryOptionsHandler(
    IApplicationDbContext applicationDbContext,
    IMapper mapper,
    IApplicationSieveProcessor sieveProcessor,
    IUserHttpContextAccessor userHttpContextAccessor)
    : IRequestHandler<GetJobsQueryOptions, PagedList<JobVacancyListDto>>
{
    public async Task<PagedList<JobVacancyListDto>> Handle(GetJobsQueryOptions request,
        CancellationToken cancellationToken)
    {
        var jobs = applicationDbContext.JobVacancies
            .Include(j => j.Category)
            .Include(j => j.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .Include(j => j.Tags)
            .ThenInclude(t => t.Tag)
            .Where(j => j.Slug != CommonVacanciesSlugs.NoVacancySlug)
            .AsNoTracking();


        var userRoles = userHttpContextAccessor.GetUserRoles();
        if (!userRoles.Any() || !userHttpContextAccessor.IsAuthenticated())
        {
            DateTime now = DateTime.Now;
            jobs = jobs.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        if (request.Tags is not null)
        {
            var tags = request.Tags.Split(',').Select(tag => tag.Trim());;
            jobs = jobs.Where(j => tags.Intersect(j.Tags.Select(t => t.Tag.Name)).Count() == tags.Count());
        }

        PagedList<JobVacancyListDto> result = sieveProcessor.ApplyAdnGetPagedList(request,
            jobs, mapper.Map<JobVacancyListDto>);
        return await Task.FromResult(result);
    }
}