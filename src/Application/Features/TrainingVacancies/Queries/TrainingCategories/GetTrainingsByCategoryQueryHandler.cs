﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.TrainingCategories;
public class GetTrainingsByCategoryQueryHandler : IRequestHandler<GetTrainingsByCategory, PagedList<TrainingVacancyListDto>>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    IApplicationSieveProcessor _sieveProcessor;
    public GetTrainingsByCategoryQueryHandler(IApplicationDbContext context, IMapper mapper, IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<TrainingVacancyListDto>> Handle(GetTrainingsByCategory request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.Include(c => c.Vacancies).ToListAsync();
        var category = (from c in categories
                        where c.Slug == request.CategorySlug
                        select c).FirstOrDefault();

        var trainings = from t in category.Vacancies
                        where t is TrainingVacancy
                        select t as TrainingVacancy;

        PagedList<TrainingVacancyListDto> trainingCategory = _sieveProcessor.ApplyAdnGetPagedList(request,
            trainings.AsQueryable(), _mapper.Map<TrainingVacancyListDto>);

        return trainingCategory;
    }
}
