using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;
public class GetVacancyCategoryByIdQueryHandler : IRequestHandler<GetByIdVacancyCategoryQuery, VacancyCategoryResponce>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVacancyCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<VacancyCategoryResponce> Handle(GetByIdVacancyCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FindAsync(new object[] { request.Id }, cancellationToken);
        _ = category ?? throw new NotFoundException(nameof(category), request.Id);
        return _mapper.Map<VacancyCategoryResponce>(category);
    }
}
