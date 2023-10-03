using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries;
public class GetInternshipCategoriesQueryHandler : IRequestHandler<GetInternshipCategoriesQuery, List<InternshipCategoriesResponce>>
{
    IApplicationDbContext _context;
    IMapper _mapper;
    public GetInternshipCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<InternshipCategoriesResponce>> Handle(GetInternshipCategoriesQuery request, CancellationToken cancellationToken)
    {
        var Internships =(await _context.Internships.ToListAsync()).AsEnumerable();
        if (request.CheckDate)
        {
            DateTime now = DateTime.UtcNow;
            Internships = Internships.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }
        var sortedInternships = from t in Internships
                                group t by t.CategoryId;
        var internshipWithCategory = new List<InternshipCategoriesResponce>();
        var categories = await _context.Categories.ToListAsync();
        foreach (var Internship in sortedInternships)
        {
            var category = categories.Where(c => c.Id == Internship.Key).FirstOrDefault();

            internshipWithCategory.Add(new InternshipCategoriesResponce
            {
                CategoryId = Internship.Key,
                Category = _mapper.Map<CategoryResponse>(category),
                InternshipCount = Internship.Count()
            });
        }
        return internshipWithCategory;
    }
}
