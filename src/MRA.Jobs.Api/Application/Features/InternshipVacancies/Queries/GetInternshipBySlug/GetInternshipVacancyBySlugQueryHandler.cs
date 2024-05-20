using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipBySlug;

public class GetInternshipVacancyBySlugQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentUserService currentUser)
    : IRequestHandler<GetInternshipVacancyBySlugQuery, InternshipVacancyResponse>
{
    public async Task<InternshipVacancyResponse> Handle(GetInternshipVacancyBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var internship = await context.Internships
            .Include(i => i.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .Include(i => i.Tags).ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken: cancellationToken);
        _ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Slug);

        internship.History = await context.VacancyTimelineEvents.Where(t => t.VacancyId == internship.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var mapped = mapper.Map<InternshipVacancyResponse>(internship);
        mapped.IsApplied = await context.Applications.AnyAsync(s =>
                s.ApplicantUsername == currentUser.GetUserName() && s.VacancyId == internship.Id,
            cancellationToken: cancellationToken);

        return mapped;
    }
}