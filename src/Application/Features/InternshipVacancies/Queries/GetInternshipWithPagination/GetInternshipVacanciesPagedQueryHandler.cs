using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipWithPagination;
public class GetInternshipVacanciesPagedQueryHandler : IRequestHandler<PaggedListQuery<InternshipVacancyListResponce>, PaggedList<InternshipVacancyListResponce>>
{
    private readonly IApplicationDbContext _context;
    private readonly IApplicationSieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;

    public GetInternshipVacanciesPagedQueryHandler(IApplicationDbContext context, IApplicationSieveProcessor sieveProcessor, IMapper mapper)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }
    public async Task<PaggedList<InternshipVacancyListResponce>> Handle(PaggedListQuery<InternshipVacancyListResponce> request, CancellationToken cancellationToken)
    {
        var result = _sieveProcessor.ApplyAdnGetPaggedList(request, _context.Internships.AsNoTracking(), _mapper.Map<InternshipVacancyListResponce>);
        return await Task.FromResult(result);
    }
}
