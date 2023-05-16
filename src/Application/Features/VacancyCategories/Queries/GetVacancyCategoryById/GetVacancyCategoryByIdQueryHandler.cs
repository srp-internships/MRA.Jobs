using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;
public class GetVacancyCategoryByIdQueryHandler : IRequestHandler<GetByIdVacancyCategoryQuery, VacancyCategoryListDTO>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetVacancyCategoryByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<VacancyCategoryListDTO> Handle(GetByIdVacancyCategoryQuery request, CancellationToken cancellationToken)
    {
        var vacancyCategory = await _dbContext.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
        _ = vacancyCategory ?? throw new NotFoundException(nameof(vacancyCategory), request.Id);
        return _mapper.Map<VacancyCategoryListDTO>(vacancyCategory);
    }
}
