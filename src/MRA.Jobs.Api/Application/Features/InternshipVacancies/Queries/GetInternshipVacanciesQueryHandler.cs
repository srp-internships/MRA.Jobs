using System;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries;

    public class GetInternshipVacanciesQueryHandler : IRequestHandler<GetInternshipsQueryOptions,
        PagedList<InternshipVacancyListResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetInternshipVacanciesQueryHandler(IApplicationDbContext context, IMapper mapper,
        IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<PagedList<InternshipVacancyListResponse>> Handle(GetInternshipsQueryOptions request, CancellationToken cancellationToken)
    {
        var internships = (await _context.InternshipVacancies.Include(t => t.Category).Include(t => t.VacancyQuestions).ToListAsync()).AsEnumerable();

        if (request.CategorySlug is not null)
        {
            internships = internships.Where(t => t.Category.Slug == request.CategorySlug);
        }
        else if (request.SearchText is not null)
        {
            internships = internships.Where(t => t.Title.ToLower().Trim().Contains(request.SearchText.ToLower().Trim()));
        }

        if (request.CheckDate)
        {
            DateTime now = DateTime.Now;
            internships = internships.Where(t => t.PublishDate <= now && t.EndDate >= now);
        }

        PagedList<InternshipVacancyListResponse> result = _sieveProcessor.ApplyAdnGetPagedList(request,
            internships.AsQueryable(), _mapper.Map<InternshipVacancyListResponse>);
        return await Task.FromResult(result);
    }
}


