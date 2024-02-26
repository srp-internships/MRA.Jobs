using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternships;

public class GetInternshipsQueryOptionsHandler(
    IApplicationDbContext context,
    IApplicationSieveProcessor sieveProcessor,
    IMapper mapper,
    IUserHttpContextAccessor userHttpContextAccessor)
    : IRequestHandler<GetInternshipsQueryOptions, PagedList<InternshipVacancyListResponse>>
{
    public async Task<PagedList<InternshipVacancyListResponse>> Handle(GetInternshipsQueryOptions request,
        CancellationToken cancellationToken)
    {
        var internships = (await context.Internships
                .Include(j => j.Category)
                .Include(j => j.VacancyQuestions)
                .Include(i => i.VacancyTasks)
                .Include(i => i.Tags).ThenInclude(t => t.Tag)
                .Where(j => j.Slug != CommonVacanciesSlugs.NoVacancySlug)
                .ToListAsync(cancellationToken: cancellationToken))
            .AsEnumerable();


        var userRoles = userHttpContextAccessor.GetUserRoles();
        if (!userRoles.Any() || !userHttpContextAccessor.IsAuthenticated())
        {
            DateTime now = DateTime.Now;
            internships = internships.Where(t => t.PublishDate <= now && t.EndDate >= now).ToList();
        }

        if (request.Tags is not null)
        {
            var tags = request.Tags.Split(',').Select(tag => tag.Trim());;
            internships = internships.Where(j => tags.Intersect(j.Tags.Select(t => t.Tag.Name)).Count() == tags.Count());
        }

        PagedList<InternshipVacancyListResponse> result = sieveProcessor.ApplyAdnGetPagedList(request,
            internships.AsQueryable(), mapper.Map<InternshipVacancyListResponse>);
        return await Task.FromResult(result);
    }
}