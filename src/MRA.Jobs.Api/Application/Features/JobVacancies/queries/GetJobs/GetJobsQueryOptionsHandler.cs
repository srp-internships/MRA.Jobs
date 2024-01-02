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
                .Where(j => j.Slug != CommonVacanciesSlugs.NoVacancySlug)
                .ToListAsync(cancellationToken: cancellationToken))
            .AsEnumerable();

        if (request.CategorySlug is not null)
        {
            jobs = jobs.Where(t => t.Category.Slug == request.CategorySlug);
        }
        else if (request.SearchText is not null)
        {
            jobs = jobs.Where(t => t.Title.ToLower().Trim().Contains(request.SearchText.ToLower().Trim()));
        }

        var userRoles = userHttpContextAccessor.GetUserRoles();
        if (userRoles.Any(role => role == "Applicant"))
            request.CheckDate = true;


        if (request.CheckDate)
        {
            DateTime now = DateTime.Now;
            jobs = jobs.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        PagedList<JobVacancyListDto> result = sieveProcessor.ApplyAdnGetPagedList(request,
            jobs.AsQueryable(), mapper.Map<JobVacancyListDto>);
        return await Task.FromResult(result);
    }
}