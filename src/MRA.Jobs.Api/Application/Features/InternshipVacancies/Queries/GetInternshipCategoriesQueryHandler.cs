using System;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipBySlug;

    public class GetInternshipCategoriesQueryHandler : IRequestHandler<GetInternshipCategoriesQuery, List<InternshipCategoriesResponse>>
    {
        IApplicationDbContext _context;
        IMapper _mapper;
        public GetInternshipCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
     

    public async Task<List<InternshipCategoriesResponse>> Handle(GetInternshipCategoriesQuery request, CancellationToken cancellationToken)
    {
        var internships = (await _context.InternshipVacancies.ToListAsync()).AsEnumerable();

        if (request.CheckDate)
        {
            DateTime now = DateTime.UtcNow;
            internships = internships.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        var sortedInternships = from t in internships
                              group t by t.CategoryId;

        var internshipsWithCategory = new List<InternshipCategoriesResponse>();
        var categories = await _context.Categories.ToListAsync();

        foreach (var internship in sortedInternships)
        {
            var category = categories.Where(c => c.Id == internship.Key).FirstOrDefault();

            internshipsWithCategory.Add(new InternshipCategoriesResponse
            {
                CategoryId = internship.Key,
                Category = _mapper.Map<CategoryResponse>(category),
               InternshipsCount = internship.Count()
            });
        }
        return internshipsWithCategory;
    }
}

