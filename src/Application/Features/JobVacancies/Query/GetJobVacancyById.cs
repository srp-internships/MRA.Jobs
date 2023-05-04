using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies.Query;
public class GetJobVacancyById : IRequestHandler<GetJobVacancyByIdQuery, JobVacancyFullResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetJobVacancyById(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<JobVacancyFullResponse> Handle(GetJobVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _context.JobVacancies
            .FirstOrDefaultAsync(j => j.Id == request.Id);

        return _mapper.Map<JobVacancyFullResponse>(jobVacancy);
    }
}
