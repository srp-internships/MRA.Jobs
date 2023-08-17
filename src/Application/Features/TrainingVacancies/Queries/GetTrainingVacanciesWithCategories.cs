using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingVacanciesWithCategories : IRequestHandler<GetTrainingVacancyWithCategoriesQuery, List<TrainingVacancyWithCategoryDto>>
{
    IApplicationDbContext _context;

    public GetTrainingVacanciesWithCategories(IApplicationDbContext context)
    {
        _context = context;

    }

    public async Task<List<TrainingVacancyWithCategoryDto>> Handle(GetTrainingVacancyWithCategoriesQuery request, CancellationToken cancellationToken)
    {
        var trainings = await _context.TrainingVacancies.ToListAsync();

        var sortedTrainings = (from t in trainings
                               group t by t.CategoryId).ToList();

        var trainingsWithCategory = new List<TrainingVacancyWithCategoryDto>();

        foreach (var training in sortedTrainings)
        {
            var categories = await _context.Categories.ToListAsync();
            trainingsWithCategory.Add(new TrainingVacancyWithCategoryDto
            {
                CategoryId = training.Key,
                CategoryName = categories.FirstOrDefault(c => c.Id == training.Key).Name,
                Trainings = training.ToList()
            });
        }
        return trainingsWithCategory;
    }
}
