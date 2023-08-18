using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplication;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

public class UpdateApplicationCommandHadler : IRequestHandler<UpdateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;

    public UpdateApplicationCommandHadler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService, ISlugGeneratorService slugService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
    }

    public async Task<Guid> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _context.Applications
            .Include(a => a.Applicant)
            .Include(a => a.Vacancy)
            .FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug); ;

        _mapper.Map(request, application);
        application.Slug = _slugService.GenerateSlug($"{application.Applicant.Slug}-{application.Vacancy.Slug}");

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Application vacancy updated",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return application.Id;
    }
}
