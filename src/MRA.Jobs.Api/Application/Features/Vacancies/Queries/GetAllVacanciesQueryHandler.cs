using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacancies.Queries;
using MRA.Jobs.Application.Contracts.Vacancies.Responses;

namespace MRA.Jobs.Application.Features.Vacancies.Queries;

public class GetAllVacanciesQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetAllVacanciesQuery, List<VacancyDto>>
{
    public async Task<List<VacancyDto>> Handle(GetAllVacanciesQuery request, CancellationToken cancellationToken)
    {
        var vacancies = await dbContext.Vacancies.ToListAsync(cancellationToken);
        return mapper.Map<List<VacancyDto>>(vacancies.OrderBy(v => v.Title));
    }
}