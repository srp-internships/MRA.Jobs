﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetTrainingCategoriesQuery, List<TrainingCategoriesResponce>>
{
    public async Task<List<TrainingCategoriesResponce>> Handle(GetTrainingCategoriesQuery request, CancellationToken cancellationToken)
    {
        var trainings = (await context.TrainingVacancies.ToListAsync()).AsEnumerable();

        if (request.CheckDate)
        {
            DateTime now = DateTime.Now;
            trainings = trainings.Where(t => t.PublishDate.AddDays(-1) <= now && t.EndDate >= now);
        }

        var sortedTrainings = from t in trainings
                              group t by t.CategoryId;

        var trainingsWithCategory = new List<TrainingCategoriesResponce>();
        var categories = await context.Categories.ToListAsync();

        foreach (var training in sortedTrainings)
        {
            var category = categories.Where(c => c.Id == training.Key).FirstOrDefault();

            trainingsWithCategory.Add(new TrainingCategoriesResponce
            {
                CategoryId = training.Key,
                Category = mapper.Map<CategoryResponse>(category),
                TrainingsCount = training.Count()
            });
        }
        return trainingsWithCategory;
    }
}
