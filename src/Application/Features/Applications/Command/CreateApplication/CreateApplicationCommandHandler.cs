﻿using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateApplicationCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId);
        _ = applicant ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);

        var application = _mapper.Map<Application>(request);
        application.Slug = GenerateSlug(applicant, vacancy);

        await _context.Applications.AddAsync(application, cancellationToken);

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Job vacancy created",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };

        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return application.Id;

    }

    private string GenerateSlug(Applicant applicant, Vacancy vacancy)
    {
        //Here instead of the applicant.Firstname should be used applicnat.Username,
        //beacuse the applicant model should be redesigned, i used Firstname temparoraly.
        return $"{applicant.FirstName.ToLower().Trim()}-{vacancy.Title.ToLower().Trim()}";
    }
}
