﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;
public class GetInternshipVacancyBySlugQueryHandler : IRequestHandler<GetInternshipVacancyBySlugQuery, InternshipVacancyResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInternshipVacancyBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<InternshipVacancyResponse> Handle(GetInternshipVacancyBySlugQuery request, CancellationToken cancellationToken)
    {
        // var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken);
        var internship = await _context.InternshipVacancies
            .Include(i => i.History)
            .Include(i => i.VacancyQuestions)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug);
        _ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Slug);
        return _mapper.Map<InternshipVacancyResponse>(internship);
    }
}