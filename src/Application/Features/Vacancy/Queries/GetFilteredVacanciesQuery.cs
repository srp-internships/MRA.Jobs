using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Application.Features.Vacancy.Queries;

using MRA.Jobs.Application.Contracts.Vacancy.Queries;
using MRA.Jobs.Domain.Entities;
public class GetFilteredVacanciesQueryHandler : IRequestHandler<GetFilteredVacanciesQuery, IEnumerable<Vacancy>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetFilteredVacanciesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Vacancy>> Handle(GetFilteredVacanciesQuery request, CancellationToken cancellationToken)
    {

        var vacancies = await _dbContext.Vacancies
            .Where(v => (!request.CategoryId.HasValue || v.CategoryId == request.CategoryId.Value)
                && (string.IsNullOrEmpty(request.Title) || v.Title.Contains(request.Title)))
            .OfType<JobVacancy>()
            .Where(jv => (!request.RequiredYearOfExperience.HasValue || jv.RequiredYearOfExperience == request.RequiredYearOfExperience.Value)
                && (!request.WorkSchedule.HasValue || jv.WorkSchedule == request.WorkSchedule.Value))
            .ToListAsync(cancellationToken);

        return vacancies;
    }
}