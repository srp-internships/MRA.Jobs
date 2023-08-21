using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
internal class GetTrairaingVacancyBySlugSinceCheck : IRequestHandler<GetTrairaingVacancyBySlugSinceCheckDate, TrainingVacancyDetailedResponse>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTrairaingVacancyBySlugSinceCheck(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TrainingVacancyDetailedResponse> Handle(GetTrairaingVacancyBySlugSinceCheckDate request, CancellationToken cancellationToken)
    {
        DateTime datenov = DateTime.Now;
        var trainingVacancy = await _context.TrainingVacancies
            .Where(t => t.PublishDate <= datenov && t.EndDate >= datenov)
             .Include(i => i.Tags)
             .ThenInclude(t => t.Tag)
             .FirstOrDefaultAsync(i => i.Slug == request.Slug);
        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Slug);
        return _mapper.Map<TrainingVacancyDetailedResponse>(trainingVacancy);
    }
}
