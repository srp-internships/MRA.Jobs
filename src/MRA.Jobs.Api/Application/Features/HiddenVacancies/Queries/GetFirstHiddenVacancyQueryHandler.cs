using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Queries;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;

namespace MRA.Jobs.Application.Features.HiddenVacancies.Queries;

public class GetFirstHiddenVacancyQueryHandler
    (IApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<GetFirstHiddenVacancyQuery,
        HiddenVacancyResponse>
{
    public async Task<HiddenVacancyResponse> Handle(GetFirstHiddenVacancyQuery request,
        CancellationToken cancellationToken)
    {
        var vacancy =
            await dbContext.HiddenVacancies.FirstOrDefaultAsync(x => x.Slug == "hidden_vacancy", cancellationToken);

        if (vacancy == null)
            throw new NullReferenceException();

        return mapper.Map<HiddenVacancyResponse>(vacancy);
    }
}