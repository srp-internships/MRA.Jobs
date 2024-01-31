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

        if (request.CategorySlug is not null)
            trainings = trainings.Where(t => t.Category.Slug == request.CategorySlug);
        
        if (request.SearchText is not null)
            trainings = trainings.Where(t => t.Title.ToLower().Trim().Contains(request.SearchText.ToLower().Trim()));
        

        var userRoles = userHttpContextAccessor.GetUserRoles();
        if (!userRoles.Any()|| !userHttpContextAccessor.IsAuthenticated())
            request.CheckDate = true;

        if (request.CheckDate)
        {
            DateTime now = DateTime.Now;
            trainings = trainings.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        PagedList<TrainingVacancyListDto> result = sieveProcessor.ApplyAdnGetPagedList(request,
            trainings.AsQueryable(), mapper.Map<TrainingVacancyListDto>);
        return await Task.FromResult(result);
    }
}