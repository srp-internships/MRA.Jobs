using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.TrainingCategories;
public class GetTrainingCategoriesByCategoryNameQueryHandler : IRequestHandler<GetTrainingCategoriesByCategoryNameQuery, TrainingVacancyWithCategoryDto>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    public GetTrainingCategoriesByCategoryNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<TrainingVacancyWithCategoryDto> Handle(GetTrainingCategoriesByCategoryNameQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.Include(c => c.Vacancies).ToListAsync();
        var category = (from c in categories
                        where c.Slug == request.CategorySlug
                        select c).FirstOrDefault();

        var trainings = (from t in category.Vacancies
                         where t is TrainingVacancy
                         select t as TrainingVacancy).ToList();

        var trainingCategory = new TrainingVacancyWithCategoryDto
        {
            CategoryId = category.Id,
            Category = _mapper.Map<CategoryResponse>(category),
            Trainings = trainings.Select(t => _mapper.Map<TrainingVacancyListDto>(t)).ToList()
        };

        return trainingCategory;
    }
}
