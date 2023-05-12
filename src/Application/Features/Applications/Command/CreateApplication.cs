using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Domain.Entities;
using System;

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
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId, cancellationToken) 
             ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);
        
        var application = _mapper.Map<Application>(request);

        var timelineEvent = new ApplicationTimelineEvent
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Application created",
            CreateBy = _currentUserService.UserId
        };
       
        _context.Applications.Add(application);
        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return application.Id;

    }
}

public class CreateApplicationCommandValidator: AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(v => v.CoverLetter)
            .NotEmpty()
            .MinimumLength(150);
        RuleFor(v => v.ResumeUrl)
            .NotEmpty();
    }
}
