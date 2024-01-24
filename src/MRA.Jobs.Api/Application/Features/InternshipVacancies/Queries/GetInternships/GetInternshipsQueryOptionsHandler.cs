using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternships;

public class GetInternshipsQueryOptionsHandler(IApplicationDbContext context,
        IApplicationSieveProcessor sieveProcessor,
        IMapper mapper, IUserHttpContextAccessor userHttpContextAccessor) 
    : IRequestHandler<GetInternshipsQueryOptions, PagedList<InternshipVacancyListResponse>>
{
    public async Task<PagedList<InternshipVacancyListResponse>> Handle(GetInternshipsQueryOptions request, CancellationToken cancellationToken)
    {
        var internships = (await context.Internships
                .Include(j => j.Category)
                .Include(j => j.VacancyQuestions)
                .Include(i => i.VacancyTasks)
                .Where(j => j.Slug != CommonVacanciesSlugs.NoVacancySlug)
                .ToListAsync(cancellationToken: cancellationToken))
            .AsEnumerable();

        if (request.CategorySlug is not null)
            internships = internships.Where(t => t.Category.Slug == request.CategorySlug);
        
        if (request.SearchText is not null)
            internships = internships.Where(t => t.Title.ToLower().Trim().Contains(request.SearchText.ToLower().Trim()));
        
        var userRoles = userHttpContextAccessor.GetUserRoles();
        if (!userRoles.Any()|| !userHttpContextAccessor.IsAuthenticated())
            request.CheckDate = true;

        if (request.CheckDate)
        {
            DateTime now = DateTime.Now;
            internships = internships.Where(t => t.PublishDate <= now && t.EndDate >= now).ToList();
        }

        PagedList<InternshipVacancyListResponse> result = sieveProcessor.ApplyAdnGetPagedList(request,
            internships.AsQueryable(), mapper.Map<InternshipVacancyListResponse>);
        return await Task.FromResult(result);
    }
}