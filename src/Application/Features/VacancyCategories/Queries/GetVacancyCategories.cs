using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries;
public class GetVacancyCategoriesQueryHandler : IRequestHandler<GetVacancyCategoriesQuery, List<VacancyCategoryResponce>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVacancyCategoriesQueryHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<VacancyCategoryResponce>> Handle(GetVacancyCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories  =await _context.Categories.
            Include(s=>s.EducationVacancies)
            .Include(s=>s.JobVacancies)
            .ToListAsync();

        return _mapper.
            Map<List<VacancyCategoryResponce>>(categories);
    }
}
