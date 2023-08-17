using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.TrainingCategories;
public class GetTrainingCategoriesQueryHandler : IRequestHandler<GetTrainingVacancyWithCategoriesQuery, List<TrainingVacancyWithCategoryDto>>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    public GetTrainingCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<TrainingVacancyWithCategoryDto>> Handle(GetTrainingVacancyWithCategoriesQuery request, CancellationToken cancellationToken)
    {
        var trainings = await _context.TrainingVacancies.ToListAsync();

        var sortedTrainings = (from t in trainings
                               group t by t.CategoryId).ToList();

        var trainingsWithCategory = new List<TrainingVacancyWithCategoryDto>();
        var categories = await _context.Categories.ToListAsync();

        foreach (var training in sortedTrainings)
        {
            var category = (from c in categories
                            where c.Id == training.Key
                            select c).FirstOrDefault();
            trainingsWithCategory.Add(new TrainingVacancyWithCategoryDto
            {
                CategoryId = training.Key,
                Category = _mapper.Map<CategoryResponse>(category),
                Trainings = training.Select(t => _mapper.Map<TrainingVacancyListDto>(t)).ToList()
            });
        }
        return trainingsWithCategory;
    }
}
