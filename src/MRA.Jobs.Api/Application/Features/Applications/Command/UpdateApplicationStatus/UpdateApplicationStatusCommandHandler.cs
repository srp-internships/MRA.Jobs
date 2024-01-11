namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplicationStatus;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Domain.Enums;

public class UpdateApplicationStatusCommandHandler(
    IApplicationDbContext dbContext,
    IDateTime dateTime,
    ICurrentUserService currentUserService,
    IHtmlService htmlService,
    IidentityService identityService)
    : IRequestHandler<UpdateApplicationStatus, bool>
{

    public async Task<bool> Handle(UpdateApplicationStatus request, CancellationToken cancellationToken)
    {
        var application = await dbContext.Applications.FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug); ;

        application.Status = (ApplicationStatus)request.StatusId;

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            Application = application,
            EventType = TimelineEventType.Updated,
            Time = dateTime.Now,
            Note = $"Application status changed: {application.Status}",
            CreateBy = currentUserService.GetUserId() ?? Guid.Empty
        };
        await dbContext.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        switch (application.Status)
        {
            case ApplicationStatus.Approved:
                await htmlService.EmailApproved( await identityService.ApplicantDetailsInfo(application.ApplicantUsername));
                break;
            case ApplicationStatus.Rejected:
                await htmlService.EmailRejected(await identityService.ApplicantDetailsInfo(application.ApplicantUsername));
                break;
        }

        return true;
    }
}
