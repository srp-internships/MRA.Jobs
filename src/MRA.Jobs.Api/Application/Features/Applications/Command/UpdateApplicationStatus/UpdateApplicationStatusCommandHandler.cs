using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MRA.Configurations.Common.Interfaces.Services;
using MRA.Configurations.OsonSms.SmsService;
using MRA.Jobs.Application.Contracts.JobVacancies;
using ValidationException = MRA.Jobs.Application.Common.Exceptions.ValidationException;

namespace MRA.Jobs.Application.Features.Applications.Command.UpdateApplicationStatus;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Domain.Enums;

public class UpdateApplicationStatusCommandHandler(
    IApplicationDbContext dbContext,
    IDateTime dateTime,
    ICurrentUserService currentUserService,
    IHtmlService htmlService,
    IidentityService identityService,
    ISmsService smsService,
    ILogger<SmsService> logger,
    IConfiguration configuration)
    : IRequestHandler<UpdateApplicationStatus, bool>
{
    public async Task<bool> Handle(UpdateApplicationStatus request, CancellationToken cancellationToken)
    {
        var application =
            await dbContext.Applications.FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug);

        if (application.Status == (ApplicationStatus)request.StatusId)
        {
            return true;
        }

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
        var applicant = await identityService.ApplicantDetailsInfo(application.ApplicantUsername);

        if (application.Slug.Contains(CommonVacanciesSlugs.NoVacancySlug))
            return true;

        switch (application.Status)
        {
            case ApplicationStatus.Approved:
                await htmlService.EmailApproved(applicant, application.Slug);
                try
                {
                    await smsService.SendSmsAsync(applicant.PhoneNumber,
                        configuration["SmsTemplates:Approved"]
                            ?.Replace("{FirstName}", applicant.FirstName)
                            .Replace("{LastName}", applicant.LastName));
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }

                break;
            case ApplicationStatus.Rejected:
                await htmlService.EmailRejected(applicant, application.Slug);
                try
                {
                    await smsService.SendSmsAsync(applicant.PhoneNumber,
                        configuration["SmsTemplates:Rejected"]
                            ?.Replace("{FirstName}", applicant.FirstName)
                            .Replace("{LastName}", applicant.LastName));
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }

                break;
        }

        return true;
    }
}