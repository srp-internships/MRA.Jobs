using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries;

public class GetByIdVacancyCategoriesQueryHandler : IRequestHandler<GetByIdVacancyCategoryQuery, VacancyCategoryResponce>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetByIdVacancyCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<VacancyCategoryResponce> Handle(GetByIdVacancyCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync();

        return _mapper
            .Map<VacancyCategoryResponce>(category);
    }
}

