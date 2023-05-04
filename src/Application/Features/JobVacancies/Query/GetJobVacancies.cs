using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.Query;
public class GetJobVacancies : IRequestHandler<GetJobVacanciesQuery, List<JobVacancyResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetJobVacancies(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<JobVacancyResponse>> Handle(GetJobVacanciesQuery request, CancellationToken cancellationToken)
    {
        var jobVacancies = await _context.JobVacancies
            .ToListAsync();

        return _mapper.Map<List<JobVacancyResponse>>(jobVacancies);
    }
}
