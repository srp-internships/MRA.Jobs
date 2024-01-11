using MRA.Jobs.Application.Contracts.JobVacancies;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

using Common.Interfaces;
using Common.Security;
using Common.SlugGeneratorService;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

public class CreateApplicationCommandHandler(
    IApplicationDbContext context,
    IMapper mapper,
    IDateTime dateTime,
    ICurrentUserService currentUserService,
    ISlugGeneratorService slugService,
    MRA.Configurations.Common.Interfaces.Services.IEmailService emailService,
    IHtmlService htmlService,
    ICvService cvService,
    IVacancyTaskService vacancyTaskService,
    IidentityService identityService,
    IConfiguration configuration)
    : IRequestHandler<CreateApplicationCommand, Guid>
{
    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        //get vacancy using request slug
        var vacancy =
            await context.Vacancies.FirstOrDefaultAsync(s => s.Slug == request.VacancySlug, cancellationToken) ??
            throw new NotFoundException($"vacancy with slug {request.VacancySlug} not found");
        //get vacancy using request slug

        //create application instance
        var application = mapper.Map<Application>(request);
        application.Slug = GenerateSlug(currentUserService.GetUserName(), vacancy);

        await ThrowIfApplicationExist(application.Slug,
            request.VacancySlug); //it will work if vacancySlug isn't equals "no_vacancy"

        application.VacancyId = vacancy.Id;
        application.ApplicantId = currentUserService.GetUserId() ?? Guid.Empty;
        application.ApplicantUsername = currentUserService.GetUserName() ?? string.Empty;

        //create application instance

        await context.Applications.AddAsync(application, cancellationToken);

        await vacancyTaskService.CheckVacancyTasksAsync(application.Id, application.TaskResponses,
            cancellationToken);

        ApplicationTimelineEvent timelineEvent = new()
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = dateTime.Now,
            CreateBy = currentUserService.GetUserId() ?? Guid.Empty
        };

        string emailHtmlBody;
        string emailSubject;
        if (vacancy.Slug == CommonVacanciesSlugs.NoVacancySlug)
        {
            emailHtmlBody = htmlService.GenerateApplyVacancyContent_NoVacancy(configuration["HostName:SvPath"],
                application.Slug, await cvService.GetCvByCommandAsync(ref request), request);
            emailSubject = "New No Vacancy apply";

            timelineEvent.Note = "Applied NoVacancy";
        }
        else
        {
            emailHtmlBody = htmlService.GenerateApplyVacancyContent_CreateApplication(configuration["HostName:SvPath"],
                application.Slug, vacancy.Title, await cvService.GetCvByCommandAsync(ref request),
                await identityService.ApplicantDetailsInfo());
            emailSubject = "New Apply";
            timelineEvent.Note = "Applied vacancy";
        }

        application.CV = await cvService.GetCvByCommandAsync(ref request);
        await context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await emailService.SendEmailAsync(new[] { vacancy.CreatedByEmail }, emailHtmlBody, emailSubject);


        return application.Id;
    }

    private async Task ThrowIfApplicationExist(string applicationSlug, string vacancySlug)
    {
        if (vacancySlug != CommonVacanciesSlugs.NoVacancySlug)
        {
            if (await context.Applications.FirstOrDefaultAsync(a => a.Slug.Equals(applicationSlug)) != null)
            {
                throw new ConflictException("Duplicate Apply. You have already submitted your application!");
            }
        }
    }

    private string GenerateSlug(string username, Vacancy vacancy)
    {
        return slugService.GenerateSlug($"{username}-{vacancy.Slug}");
    }
}