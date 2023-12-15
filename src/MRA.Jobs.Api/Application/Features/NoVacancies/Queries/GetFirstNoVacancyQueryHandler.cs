using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.NoVacancies.Queries;
using MRA.Jobs.Application.Contracts.NoVacancies.Responses;

namespace MRA.Jobs.Application.Features.NoVacancies.Queries;

public class GetFirstNoVacancyQueryHandler
    (IApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<GetFirstNoVacancyQuery,
        NoVacancyResponse>
{
    public async Task<NoVacancyResponse> Handle(GetFirstNoVacancyQuery request,
        CancellationToken cancellationToken)
    {
        var vacancy =
            await dbContext.HiddenVacancies.FirstOrDefaultAsync(x => x.Slug == "no_vacancy", cancellationToken);

        if (vacancy == null)
            throw new NullReferenceException();

        return mapper.Map<NoVacancyResponse>(vacancy);
    }
}