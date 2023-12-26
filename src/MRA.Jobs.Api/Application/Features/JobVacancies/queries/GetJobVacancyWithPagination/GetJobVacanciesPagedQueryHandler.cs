﻿using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyWithPagination;

public class GetJobVacanciesPagedQueryHandler : IRequestHandler<PagedListQuery<JobVacancyListDto>, PagedList<JobVacancyListDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetJobVacanciesPagedQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public async Task<PagedList<JobVacancyListDto>> Handle(PagedListQuery<JobVacancyListDto> request,
        CancellationToken cancellationToken)
    {
        PagedList<JobVacancyListDto> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            _dbContext.JobVacancies
            .Include(j => j.Category)
            .Include(j => j.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .Where(j=>j.Slug!=CommonVacanciesSlugs.NoVacancySlug)
            .AsNoTracking(),
            _mapper.Map<JobVacancyListDto>);

        var user = await _dbContext.Categories.ToListAsync(cancellationToken: cancellationToken);
        
        return await Task.FromResult(result);
    }
}