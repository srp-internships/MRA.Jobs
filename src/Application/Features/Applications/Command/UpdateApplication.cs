using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Domain.Entities;

public class UpdateApplicationCommandHadler : IRequestHandler<UpdateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public UpdateApplicationCommandHadler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,  ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Applications.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Application), request.Id);

        var applicant = await _context.Applicants.FindAsync(request.ApplicantId, cancellationToken)
             ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);

        var application = _mapper.Map<Application>(request);
        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Job vacancy updated",
            CreateBy = _currentUserService.UserId
        };
        _context.Applications.Update(application);
        await _context.SaveChangesAsync(cancellationToken);
        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);

        return entity.Id;
    }
}
