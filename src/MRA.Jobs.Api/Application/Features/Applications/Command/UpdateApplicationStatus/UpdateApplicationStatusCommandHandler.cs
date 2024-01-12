using Microsoft.Extensions.Logging;
using MRA.Configurations.Common.Interfaces.Services;
using MRA.Configurations.OsonSms.SmsService;
using MRA.Jobs.Application.Contracts.JobVacancies;

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
    ILogger<SmsService> logger)
    : IRequestHandler<UpdateApplicationStatus, bool>
{
    public async Task<bool> Handle(UpdateApplicationStatus request, CancellationToken cancellationToken)
    {
        var application =
            await dbContext.Applications.FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug);
        ;

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
                        $"{applicant.FirstName} {applicant.LastName}, Ваше резюме одобрено, мы скоро с вами свяжемся, чтобы пригласить вас на собеседование.");
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }

                break;
            case ApplicationStatus.Rejected:
                await htmlService.EmailRejected(applicant,application.Slug);
                try
                {
                    await smsService.SendSmsAsync(applicant.PhoneNumber,
                        $"{applicant.FirstName} {applicant.LastName}, спасибо за интерес к нашей вакансии. К сожалению, ваше резюме отклонено.");
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