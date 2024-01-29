namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplication;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplication;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

public class UpdateApplicationCommandHandler(
    IApplicationDbContext context,
    IMapper mapper,
    IDateTime dateTime,
    ICurrentUserService currentUserService,
    ISlugGeneratorService slugService)
    : IRequestHandler<UpdateApplicationCommand, Guid>
{
    public async Task<Guid> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await context.Applications
            .Include(a => a.Vacancy)
            .FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug); ;

        mapper.Map(request, application);
        application.Slug = slugService.GenerateSlug($"{currentUserService.GetUserName()}-{application.Vacancy.Slug}");

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Updated,
            Time = dateTime.Now,
            Note = "Application vacancy updated",
            CreateBy = currentUserService.GetUserName() ?? string.Empty
        };
        await context.ApplicationTimelineEvents.AddAsync(timelineEvent);
        await context.SaveChangesAsync(cancellationToken);

        return application.Id;
    }
}
