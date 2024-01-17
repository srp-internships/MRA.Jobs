using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;
public class GetInternshipVacancyBySlugQueryHandler : IRequestHandler<GetInternshipVacancyBySlugQuery, InternshipVacancyResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetInternshipVacancyBySlugQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<InternshipVacancyResponse> Handle(GetInternshipVacancyBySlugQuery request, CancellationToken cancellationToken)
    {
        // var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken);
        var internship = await _context.Internships
            .Include(i => i.History)
            .Include(i => i.VacancyQuestions)  
            .Include(i => i.VacancyTasks)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug);
        _ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Slug);

        var mapped =  _mapper.Map<InternshipVacancyResponse>(internship);
        mapped.IsApplied = await _context.Applications.AnyAsync(s => s.ApplicantId == _currentUser.GetUserId() && s.VacancyId == internship.Id);

        return mapped;
    }
}