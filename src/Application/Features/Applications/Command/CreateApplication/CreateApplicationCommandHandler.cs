using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateApplicationCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId);
        _ = applicant ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);

        var application = _mapper.Map<Application>(request);

        await _context.Applications.AddAsync(application, cancellationToken);

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Job vacancy created",
            CreateBy = _currentUserService.GetId().Value
        };

        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return application.Id;

    }
}
