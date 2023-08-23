using MRA.Jobs.Application.Contracts.Applications.Commands;
using Slugify;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;

    public CreateApplicationCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService, ISlugGeneratorService slugService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
    }

    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        Vacancy vacancy = await _context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);
        var application = _mapper.Map<Application>(request);

        application.Slug = GenerateSlug(_currentUserService.GetUserName(), vacancy);
        application.JobQuestions = request.JobQuestions;

        await _context.Applications.AddAsync(application, cancellationToken);

        ApplicationTimelineEvent timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Job vacancy created",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };

        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return application.Id;

    }
    private string GenerateSlug(string username, Vacancy vacancy)
    {
        return _slugService.GenerateSlug($"{username}-{vacancy.Slug}");
    }
}
