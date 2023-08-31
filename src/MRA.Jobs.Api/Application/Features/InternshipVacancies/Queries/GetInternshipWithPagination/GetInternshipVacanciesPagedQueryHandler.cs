using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipWithPagination;

public class GetInternshipVacanciesPagedQueryHandler : IRequestHandler<PagedListQuery<InternshipVacancyListResponse>,
    PagedList<InternshipVacancyListResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetInternshipVacanciesPagedQueryHandler(IApplicationDbContext context,
        IApplicationSieveProcessor sieveProcessor, IMapper mapper)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
    }

    public async Task<PagedList<InternshipVacancyListResponse>> Handle(
        PagedListQuery<InternshipVacancyListResponse> request, CancellationToken cancellationToken)
    {
        PagedList<InternshipVacancyListResponse> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            _context.Internships
                .Include(i => i.Category)
                .Include(i => i.VacancyQuestions)
                .AsNoTracking(),
            _mapper.Map<InternshipVacancyListResponse>);
        return await Task.FromResult(result);
    }
}