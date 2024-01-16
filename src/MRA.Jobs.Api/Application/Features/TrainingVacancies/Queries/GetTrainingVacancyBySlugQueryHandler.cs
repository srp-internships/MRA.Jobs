using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingVacancyBySlugQueryHandler : IRequestHandler<GetTrainingVacancyBySlugQuery, TrainingVacancyDetailedResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetTrainingVacancyBySlugQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    public async Task<TrainingVacancyDetailedResponse> Handle(GetTrainingVacancyBySlugQuery request, CancellationToken cancellationToken)
    {
        var trainingVacancy = await _context.TrainingVacancies
            .Include(i => i.History)
            .Include(i => i.VacancyTasks)
            .Include(i => i.VacancyQuestions)
            .Include(i => i.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug);

        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Slug);
        var mapped = _mapper.Map<TrainingVacancyDetailedResponse>(trainingVacancy);
        mapped.IsApplied = await _context.Applications.AnyAsync(s => s.ApplicantId == _currentUser.GetUserId());

        return mapped;
    }
}