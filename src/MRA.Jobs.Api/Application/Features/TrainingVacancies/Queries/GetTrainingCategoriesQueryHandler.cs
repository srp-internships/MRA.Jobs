﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using static Sieve.Extensions.MethodInfoExtended;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingCategoriesQueryHandler : IRequestHandler<GetTrainingCategoriesQuery, List<TrainingCategoriesResponce>>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    public GetTrainingCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<TrainingCategoriesResponce>> Handle(GetTrainingCategoriesQuery request, CancellationToken cancellationToken)
    {
        var trainings = (await _context.TrainingVacancies.ToListAsync()).AsEnumerable();

        if (request.CheckDate)
        {
            DateTime now = DateTime.UtcNow;
            trainings = trainings.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        var sortedTrainings = from t in trainings
                              group t by t.CategoryId;

        var trainingsWithCategory = new List<TrainingCategoriesResponce>();
        var categories = await _context.Categories.ToListAsync();

        foreach (var training in sortedTrainings)
        {
            var category = categories.Where(c => c.Id == training.Key).FirstOrDefault();

            trainingsWithCategory.Add(new TrainingCategoriesResponce
            {
                CategoryId = training.Key,
                Category = _mapper.Map<CategoryResponse>(category),
                TrainingsCount = training.Count()
            });
        }
        return trainingsWithCategory;
    }
}
