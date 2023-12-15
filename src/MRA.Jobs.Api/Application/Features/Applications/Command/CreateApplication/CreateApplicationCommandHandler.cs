namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

using Common.Interfaces;
using Common.Security;
using Common.SlugGeneratorService;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

public class CreateApplicationCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService, ISlugGeneratorService slugService,
        MRA.Configurations.Common.Interfaces.Services.IEmailService emailService, IHtmlService htmlService,
        ICvService cvService, IVacancyTaskService vacancyTaskService, IidentityService identityService,
        IConfiguration configuration)
    : IRequestHandler<CreateApplicationCommand, Guid>
{
    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
       var vacancy = await context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);
        var application = mapper.Map<Application>(request);

        application.Slug = GenerateSlug(currentUserService.GetUserName(), vacancy);

        if (await ApplicationExits(application.Slug))
            throw new ConflictException("Duplicate Apply. You have already submitted your application!");

        if (vacancy.Discriminator == "NoVacancy")
        {
            var count = context.Applications.Count(a => a.Slug.Contains("no_vacancy"));
            application.Slug += count + 1;
       }

        application.ApplicantId = currentUserService.GetUserId() ?? Guid.Empty;
        application.ApplicantUsername = currentUserService.GetUserName() ?? string.Empty;
        application.CV = await cvService.GetCvByCommandAsync(ref request);

        await context.Applications.AddAsync(application, cancellationToken);
        await vacancyTaskService.CheckVacancyTasksAsync(application.Id, application.TaskResponses, cancellationToken);
        ApplicationTimelineEvent timelineEvent = new()
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = dateTime.Now,
            Note = "Applied vacancy",
            CreateBy = currentUserService.GetUserId() ?? Guid.Empty
        };

        await context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        string hostName = configuration["HostName:SvPath"];
        await emailService.SendEmailAsync(new[] { vacancy.CreatedByEmail },
            htmlService.GenerateApplyVacancyContent_CreateApplication(hostName, application.Slug, vacancy.Title,
                await cvService.GetCvByCommandAsync(ref request), await identityService.ApplicantDetailsInfo()),
            "New Apply");

        return application.Id;
    }

    private async Task<bool> ApplicationExits(string applicationSlug)
    {
        var application = await context.Applications.Include(a => a.Vacancy)
            .FirstOrDefaultAsync(a => a.Slug.Equals(applicationSlug));

        if (application == null)
            return false;

        if (application.Vacancy.Discriminator == "NoVacancy" && (application.Status == ApplicationStatus.Expired ||
                                                                     application.Status == ApplicationStatus.Refused ||
                                                                     application.Status == ApplicationStatus.Rejected))
            return false;

        return true;
    }

    private string GenerateSlug(string username, Vacancy vacancy)
    {
        return slugService.GenerateSlug($"{username}-{vacancy.Slug}");
    }
}