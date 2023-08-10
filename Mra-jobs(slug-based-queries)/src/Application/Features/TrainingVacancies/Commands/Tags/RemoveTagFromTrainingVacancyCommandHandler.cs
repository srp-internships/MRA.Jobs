﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;
public class RemoveTagFromTrainingVacancyCommandHandler : IRequestHandler<RemoveTagFromTrainingVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public RemoveTagFromTrainingVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<bool> Handle(RemoveTagFromTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var trainingVacancy = await _context.TrainingVacancies
         .Include(x => x.Tags)
         .ThenInclude(t => t.Tag)
         .FirstOrDefaultAsync(x => x.Id == request.VacancyId, cancellationToken);

        if (trainingVacancy == null)
            throw new NotFoundException(nameof(trainingVacancy), request.VacancyId);

        foreach (var tagName in request.Tags)
        {
            var vacancyTag = trainingVacancy.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
                continue;

            _context.VacancyTags.Remove(vacancyTag);

            var timelineEvent = new VacancyTimelineEvent
            {
                VacancyId = trainingVacancy.Id,
                EventType = TimelineEventType.Deleted,
                Time = _dateTime.Now,
                Note = $"Removed '{tagName}' tag",
                CreateBy = _currentUserService.GetId() ?? Guid.Empty
            };
            await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);

        }
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
