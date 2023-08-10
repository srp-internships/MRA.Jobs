﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Update;
public class UpdateInternshipVacancyCommandHandler : IRequestHandler<UpdateInternshipVacancyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public UpdateInternshipVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        //var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        //_ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Id);

        //var category = await _context.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken: cancellationToken);
        //_ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        var internship = await _context.Internships
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        _ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Id);

        _mapper.Map(request, internship);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = internship.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Internship updated",
            CreateBy = _currentUserService.GetId() ?? Guid.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return internship.Id;
    }
}