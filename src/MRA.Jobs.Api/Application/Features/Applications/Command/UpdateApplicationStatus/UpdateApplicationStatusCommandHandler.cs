using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MRA.Configurations.Common.Interfaces.Services;
using MRA.Configurations.OsonSms.SmsService;
using MRA.Identity.Application.Contract.Profile.Responses;
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
    : IRequestHandler<UpdateApplicationStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var application =
            await dbContext.Applications.Include(a => a.Vacancy)
                .FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Application), request.Slug);
        if (application.Status == (ApplicationStatus)request.StatusId)
            return true;


        application.Status = (ApplicationStatus)request.StatusId;

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            Application = application,
            EventType = TimelineEventType.Updated,
            Time = dateTime.Now,
            Note = $"Application status changed: {application.Status}",
            CreateBy = currentUserService.GetUserName() ?? string.Empty
        };
        await dbContext.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var applicant = await identityService.ApplicantDetailsInfo(application.ApplicantUsername);

        if (application.Slug.Contains(CommonVacanciesSlugs.NoVacancySlug))
            return true;

        string smsTemplate = "";

        if (request.EmailVariant == SendEmailVariant.CustomEmail)
            await htmlService.EmailCustom(request.EmailSubject, request.EmailText, applicant.Email);

        if (request.EmailVariant == SendEmailVariant.AutoEmail)
        {
            switch (application.Status)
            {
                case ApplicationStatus.Approved:
                    smsTemplate = configuration["SmsTemplates:Approved"];
                    await htmlService.EmailApproved(applicant, application.Slug);
                    break;
                case ApplicationStatus.Rejected:
                    smsTemplate = configuration["SmsTemplates:Rejected"];
                    await htmlService.EmailRejected(applicant, application.Slug, application.Vacancy.Title);
                    break;
            }
        }

        await SendSms(smsTemplate, applicant);

        return true;
    }

    private async Task SendSms(string smsTemplate, UserProfileResponse applicant)
    {
        if (smsTemplate == "")
            return;

        try
        {
            await smsService.SendSmsAsync(applicant.PhoneNumber,
                smsTemplate?.Replace("{FirstName}", applicant.FirstName)
                    .Replace("{LastName}", applicant.LastName));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}