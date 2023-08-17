using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;

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
        var categories = await _context.Categories.Select(c => new { c.Id, c.Name }).ToListAsync();

        foreach (var training in sortedTrainings)
        {
            trainingsWithCategory.Add(new TrainingVacancyWithCategoryDto
            {
                CategoryId = training.Key,
                CategoryName = (from c in categories
                                where c.Id == training.Key
                                select c).FirstOrDefault().Name,
                Trainings = training.ToList()
            });
        }
        return trainingsWithCategory;
    }
}
