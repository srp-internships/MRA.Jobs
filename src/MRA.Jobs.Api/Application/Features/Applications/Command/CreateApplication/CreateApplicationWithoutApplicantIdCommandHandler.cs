using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

public class
    CreateApplicationWithoutApplicantIdCommandHandler : IRequestHandler<CreateApplicationWithoutApplicantIdCommand,
        Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;

    public CreateApplicationWithoutApplicantIdCommandHandler(IApplicationDbContext context, IMapper mapper,
        IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateApplicationWithoutApplicantIdCommand request,
        CancellationToken cancellationToken)
    {
        Vacancy vacancy = await _context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);

        var application = _mapper.Map<MRA.Jobs.Domain.Entities.Application>(request);

        // temp: Gets username from Claims
        string username = "temp";
        application.Slug = GenerateSlug(username, vacancy);

        await _context.Applications.AddAsync(application, cancellationToken);

        ApplicationTimelineEvent timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Job vacancy created",
            CreateBy = _currentUserService.GetUserId().Value
        };

        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return application.Id;
    }

    private string GenerateSlug(string username, Vacancy vacancy)
    {
        //Here instead of the applicant.Firstname should be used applicnat.Username,
        //beacuse the applicant model should be redesigned, i used Firstname temparoraly.
        return $"{username.ToLower().Trim()}-{vacancy.Title.ToLower().Trim()}";
    }
}