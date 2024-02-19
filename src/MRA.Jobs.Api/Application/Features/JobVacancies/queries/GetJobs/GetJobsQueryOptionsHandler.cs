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
        var jobs = (await applicationDbContext.JobVacancies
                .Include(j => j.Category)
                .Include(j => j.VacancyQuestions)
                .Include(i => i.VacancyTasks)
                .Include(j => j.Tags)
                .ThenInclude(t => t.Tag)
                .Where(j => j.Slug != CommonVacanciesSlugs.NoVacancySlug).AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken))
            .AsEnumerable();

        if (request.CategorySlug is not null)
            jobs = jobs.Where(t => t.Category.Slug == request.CategorySlug);

        if (request.SearchText is not null)
            jobs = jobs.Where(t => t.Title.ToLower().Trim().Contains(request.SearchText.ToLower().Trim()));


        var userRoles = userHttpContextAccessor.GetUserRoles();
        if (!userRoles.Any() || !userHttpContextAccessor.IsAuthenticated())
            request.CheckDate = true;


        if (request.CheckDate)
        {
            DateTime now = DateTime.Now;
            jobs = jobs.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        if (request.Tags is not null && request.Tags.Count > 0)
        {
            var requestTags = request.Tags.ToArray();
            var newList = new List<JobVacancy>();
            jobs = jobs.Where(j => j.Tags != null && j.Tags.Count != 0);
            foreach (var jobVacancy in jobs)
            {
                var vacancyTags = jobVacancy.Tags.Select(s => s.Tag.Name).ToArray();
                if (tagExist(vacancyTags, requestTags))
                {
                    newList.Add(jobVacancy);
                }
            }
        
            jobs = newList;
            // jobs = jobs.Where(j => j.Tags.Any(t => request.Tags.Contains(t.Tag.Name)));
        }

        PagedList<JobVacancyListDto> result = sieveProcessor.ApplyAdnGetPagedList(request,
            jobs.AsQueryable(), mapper.Map<JobVacancyListDto>);
        return await Task.FromResult(result);
    }

    private bool tagExist(string[] vacancyTags, string[] requestTags)
    {
        var vacancyTagsJoin = vacancyTags.Aggregate("", (o, s) => o + s);
        foreach (var requestTag in requestTags)
        {
            if (!vacancyTagsJoin.Contains(requestTag))
            {
                return false;
            }
        }

        return true;
    }
}