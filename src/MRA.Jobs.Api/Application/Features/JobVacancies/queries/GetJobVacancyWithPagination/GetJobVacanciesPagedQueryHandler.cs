using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyWithPagination;

public class GetJobVacanciesPagedQueryHandler(
    IApplicationDbContext dbContext,
    IApplicationSieveProcessor sieveProcessor,
    IMapper mapper)
    : IRequestHandler<PagedListQuery<JobVacancyListDto>, PagedList<JobVacancyListDto>>
{
    public async Task<PagedList<JobVacancyListDto>> Handle(PagedListQuery<JobVacancyListDto> request,
        CancellationToken cancellationToken)
    {
        var vacancies = dbContext.JobVacancies
            .Include(j => j.Category)
            .Include(j => j.VacancyQuestions)
            .Include(j => j.VacancyTasks)
            .Include(j => j.Tags)
            .ThenInclude(t => t.Tag)
            .Where(j => j.Slug != CommonVacanciesSlugs.NoVacancySlug)
            .AsNoTracking();

        PagedList<JobVacancyListDto> result = sieveProcessor.ApplyAdnGetPagedList(request,
            vacancies, mapper.Map<JobVacancyListDto>);

        return await Task.FromResult(result);
    }
}