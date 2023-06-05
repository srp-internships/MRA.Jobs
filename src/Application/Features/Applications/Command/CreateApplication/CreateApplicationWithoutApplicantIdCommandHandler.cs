using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;
using MRA.Jobs.Domain.Entities;
public class CreateApplicationWithoutApplicantIdCommandHandler : IRequestHandler<CreateApplicationWithoutApplicantIdCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateApplicationWithoutApplicantIdCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateApplicationWithoutApplicantIdCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);
        var applicant = await _context.Applicants.FindAsync(_currentUserService.GetId().Value);
        _ = applicant ?? throw new NotFoundException(nameof(Applicant), _currentUserService.GetId().Value);

        var application = _mapper.Map<Application>(request);
        application.Applicant = applicant;

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
