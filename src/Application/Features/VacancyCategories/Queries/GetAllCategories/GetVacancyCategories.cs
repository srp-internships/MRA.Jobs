using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetAllCategories;
public class GetVacancyCategoriesQueryHandler : IRequestHandler<GetVacancyCategoriesQuery, List<VacancyCategoryResponce>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVacancyCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<VacancyCategoryResponce>> Handle(GetVacancyCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.
            Include(s => s.Vacancies)
            .ToListAsync();

        return _mapper.
            Map<List<VacancyCategoryResponce>>(categories);
    }
}
