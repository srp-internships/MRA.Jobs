using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobsPagedByCategoryQueryHandler;
public class GetJobsPagedByCategoryQueryHandler : IRequestHandler<GetJobsPagedByCategoryQuery, List<JobVacancyByCategoryDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetJobsPagedByCategoryQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<JobVacancyByCategoryDTO>> Handle(GetJobsPagedByCategoryQuery request, CancellationToken cancellationToken)
    {
        DateTime datenov = DateTime.Now;
        var result = await _dbContext.JobVacancies
            .Where(j => j.PublishDate <= datenov && j.EndDate >= datenov
            && j.CategoryId == request.CatrgoryId)
           .ToListAsync(cancellationToken);

        return _mapper.Map<List<JobVacancyByCategoryDTO>>(result);

    }
}
