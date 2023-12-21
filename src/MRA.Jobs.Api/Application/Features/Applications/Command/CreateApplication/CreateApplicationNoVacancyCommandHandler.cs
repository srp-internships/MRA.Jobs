// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using MRA.Jobs.Application.Common.SlugGeneratorService;
// using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
//
// namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;
//
// public class CreateApplicationNoVacancyCommandHandler(
//     IApplicationDbContext context,
//     IMapper mapper,
//     IDateTime dateTime,
//     ICurrentUserService currentUserService,
//     ISlugGeneratorService slugService,
//     MRA.Configurations.Common.Interfaces.Services.IEmailService emailService,
//     IHtmlService htmlService,
//     ICvService cvService,
//     IidentityService identityService,
//     IConfiguration configuration) : IRequestHandler<CreateApplicationNoVacancyCommand, Guid>
// {
//     public async Task<Guid> Handle(CreateApplicationNoVacancyCommand request, CancellationToken cancellationToken)
//     {
//         var vacancy =
//             await context.NoVacancies.FirstOrDefaultAsync(x => x.Slug == "no_vacancy",
//                 cancellationToken: cancellationToken) ?? throw new Exception("Server database error");
//         var applicationsCount = context.Applications.Include(x => x.Vacancy)
//             .Count(x => x.Vacancy.Slug == "no_vacancy");
//         var application = mapper.Map<Domain.Entities.Application>(request);
//
//         application.VacancyId = vacancy.Id;
//         application.Slug = GenerateSlug(vacancy, applicationsCount + 1);
//         application.ApplicantId = currentUserService.GetUserId() ?? Guid.Empty;
//         application.ApplicantUsername = currentUserService.GetUserName() ?? string.Empty;
//         application.CV = await cvService.GetCvByCommandNoVacancyAsync(ref request);
//
//         await context.Applications.AddAsync(application, cancellationToken);
//
//         ApplicationTimelineEvent timelineEvent = new()
//         {
//             ApplicationId = application.Id,
//             EventType = TimelineEventType.Created,
//             Time = dateTime.Now,
//             Note = "Applied vacancy",
//             CreateBy = currentUserService.GetUserId() ?? Guid.Empty,
//         };
//
//         await context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
//         await context.SaveChangesAsync(cancellationToken);
//
//         string hostName = configuration["HostName:SvPath"];
//         await emailService.SendEmailAsync(new[] { vacancy.CreatedByEmail },
//             htmlService.GenerateApplyVacancyContent_NoVacancy(hostName, application.Slug, await cvService.GetCvByCommandNoVacancyAsync(ref request), request),
//             "New Apply");
//
//         return application.Id;
//     }
//
//     private string GenerateSlug(Vacancy vacancy, int number)
//     {
//         return slugService.GenerateSlug($"{vacancy.Slug}-{number}");
//     }
// }