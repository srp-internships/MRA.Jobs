using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetAllVacancyCategory;
public class GetAllVacancyCategoryQueryHandler : IRequestHandler<GetAllVacancyCategoryQuery, List<VacancyCategoryListDTO>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllVacancyCategoryQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }


    public async Task<List<VacancyCategoryListDTO>> Handle(GetAllVacancyCategoryQuery request, CancellationToken cancellationToken)
    {
       var categories = await _dbContext.Categories.ToListAsync(cancellationToken);
        return _mapper.Map<List<VacancyCategoryListDTO>>(categories);
    }
}


