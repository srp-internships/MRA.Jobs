using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;

public class GetTrainingVacancyBySlugQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentUserService currentUser)
    : IRequestHandler<GetTrainingVacancyBySlugQuery, TrainingVacancyDetailedResponse>
{
    public async Task<TrainingVacancyDetailedResponse> Handle(GetTrainingVacancyBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var trainingVacancy = await context.TrainingVacancies
            .Include(i => i.VacancyTasks)
            .Include(i => i.VacancyQuestions)
            .Include(i => i.Tags).ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken: cancellationToken);
        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Slug);

        trainingVacancy.History =
            await context.VacancyTimelineEvents.Where(t => t.VacancyId == trainingVacancy.Id)
                .ToListAsync(cancellationToken: cancellationToken);
        
        var mapped = mapper.Map<TrainingVacancyDetailedResponse>(trainingVacancy);
        mapped.IsApplied = await context.Applications.AnyAsync(s =>
            s.ApplicantUsername == currentUser.GetUserName() && s.VacancyId == trainingVacancy.Id, cancellationToken: cancellationToken);

        return mapped;
    }
}