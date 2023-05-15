using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplication;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

public class UpdateApplicationCommandHadler : IRequestHandler<UpdateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public UpdateApplicationCommandHadler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _context.Applications.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Application), request.Id);

        var applicant = await _context.Applicants.FindAsync(request.ApplicantId, cancellationToken)
             ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);

        _mapper.Map(request, application);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = application.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Application updated",
            CreateBy = _currentUserService.UserId
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return application.Id;
    }
}
