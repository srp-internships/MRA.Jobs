using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.TrainingCategories;
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
        var trainings = await _context.TrainingVacancies.ToListAsync();

        var sortedTrainings = (from t in trainings
                               group t by t.CategoryId).ToList();

        var trainingsWithCategory = new List<TrainingCategoriesResponce>();
        var categories = await _context.Categories.ToListAsync();

        foreach (var training in sortedTrainings)
        {
            var category = (from c in categories
                            where c.Id == training.Key
                            select c).FirstOrDefault();
            trainingsWithCategory.Add(new TrainingCategoriesResponce
            {
                CategoryId = training.Key,
                Category = _mapper.Map<CategoryResponse>(category),
            });
        }
        return trainingsWithCategory;
    }
}
