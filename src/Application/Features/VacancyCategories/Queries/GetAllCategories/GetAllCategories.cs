using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Features.VacancyCategories.Queries.Responce;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetAllCategories;
public  class GetAllCategories : IRequest<CategoryResponce>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllCategories(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryResponce>> GetAll()
    {
        var caterogies = await _dbContext.Categories.ToListAsync();
        return _mapper.Map<List<CategoryResponce>>(caterogies);
    }
}
