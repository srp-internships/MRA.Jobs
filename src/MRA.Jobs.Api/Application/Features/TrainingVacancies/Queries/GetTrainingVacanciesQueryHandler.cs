using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;

public class GetTrainingVacanciesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IApplicationSieveProcessor sieveProcessor,
        IUserHttpContextAccessor userHttpContextAccessor)
    : IRequestHandler<GetTrainingsQueryOptions,
        PagedList<TrainingVacancyListDto>>
{
    public async Task<PagedList<TrainingVacancyListDto>> Handle(GetTrainingsQueryOptions request,
        CancellationToken cancellationToken)
    {
        var trainings = (await context.TrainingVacancies
            .Include(t => t.Category)
            .Include(t => t.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .Include(i=>i.Tags).ThenInclude(t=>t.Tag)
            .ToListAsync(cancellationToken: cancellationToken))
            .AsEnumerable();

        var userRoles = userHttpContextAccessor.GetUserRoles();
        if (!userRoles.Any()|| !userHttpContextAccessor.IsAuthenticated())
        {
            DateTime now = DateTime.Now;
            trainings = trainings.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        if (request.Tags is not null)
        {
            var tags = request.Tags.Split(',').Select(tag => tag.Trim());;
            trainings = trainings.Where(j => tags.Intersect(j.Tags.Select(t => t.Tag.Name)).Count() == tags.Count());
        }
        
        PagedList<TrainingVacancyListDto> result = sieveProcessor.ApplyAdnGetPagedList(request,
            trainings.AsQueryable(), mapper.Map<TrainingVacancyListDto>);
        return await Task.FromResult(result);
    }
}