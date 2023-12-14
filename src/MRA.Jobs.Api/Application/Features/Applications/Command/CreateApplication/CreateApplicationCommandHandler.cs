﻿namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

using Common.Interfaces;
using Common.Security;
using Common.SlugGeneratorService;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;
    private readonly MRA.Configurations.Common.Interfaces.Services.IEmailService _emailService;
    private readonly IHtmlService _htmlService;
    private readonly ICvService _cvService;
    private readonly IVacancyTaskService _vacancyTaskService;
    private readonly IidentityService _identityService;
    private readonly IConfiguration _configuration;

    public CreateApplicationCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService, ISlugGeneratorService slugService,
        MRA.Configurations.Common.Interfaces.Services.IEmailService emailService, IHtmlService htmlService,
        ICvService cvService, IVacancyTaskService vacancyTaskService, IidentityService identityService,
        IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
        _emailService = emailService;
        _htmlService = htmlService;
        _cvService = cvService;
        _vacancyTaskService = vacancyTaskService;
        _identityService = identityService;
        _configuration = configuration;
    }

    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
       var vacancy = await _context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);
        var application = _mapper.Map<Application>(request);

        application.Slug = GenerateSlug(_currentUserService.GetUserName(), vacancy);

        if (await ApplicationExits(application.Slug))
            throw new ConflictException("Duplicate Apply. You have already submitted your application!");

        if (vacancy.Discriminator == "HiddenVacancy")
        {
            var count = _context.Applications.Count(a => a.Slug.Contains("hidden_vacancy"));
            application.Slug += count + 1;
       }

        application.ApplicantId = _currentUserService.GetUserId() ?? Guid.Empty;
        application.ApplicantUsername = _currentUserService.GetUserName() ?? string.Empty;
        application.CV = await _cvService.GetCvByCommandAsync(ref request);

        await _context.Applications.AddAsync(application, cancellationToken);
        await _vacancyTaskService.CheckVacancyTasksAsync(application.Id, application.TaskResponses, cancellationToken);
        ApplicationTimelineEvent timelineEvent = new()
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Applied vacancy",
            CreateBy = _currentUserService.GetUserId() ?? Guid.Empty
        };

        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        string hostName = _configuration["HostName:SvPath"];
        await _emailService.SendEmailAsync(new[] { vacancy.CreatedByEmail },
            _htmlService.GenerateApplyVacancyContent_CreateApplication(hostName, application.Slug, vacancy.Title,
                await _cvService.GetCvByCommandAsync(ref request), await _identityService.ApplicantDetailsInfo()),
            "New Apply");

        return application.Id;
    }

    private async Task<bool> ApplicationExits(string applicationSlug)
    {
        var application = await _context.Applications.Include(a => a.Vacancy)
            .FirstOrDefaultAsync(a => a.Slug.Equals(applicationSlug));

        if (application == null)
            return false;

        if (application.Vacancy.Discriminator == "HiddenVacancy" && (application.Status == ApplicationStatus.Expired ||
                                                                     application.Status == ApplicationStatus.Refused ||
                                                                     application.Status == ApplicationStatus.Rejected))
            return false;

        return true;
    }

    private string GenerateSlug(string username, Vacancy vacancy)
    {
        return _slugService.GenerateSlug($"{username}-{vacancy.Slug}");
    }
}