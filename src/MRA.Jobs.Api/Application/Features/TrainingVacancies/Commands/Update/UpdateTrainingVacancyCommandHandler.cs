﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Update;

public class UpdateTrainingVacancyCommandHandler : IRequestHandler<UpdateTrainingVacancyCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;
    private readonly ISlugGeneratorService _slugService;

    public UpdateTrainingVacancyCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService, IDateTime dateTime, ISlugGeneratorService slugService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
        _slugService = slugService;
    }

    public async Task<string> Handle(UpdateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var trainingVacancy = await _context.TrainingVacancies
           .Include(i => i.Category)
           .Include(i => i.VacancyQuestions)
           .Include(i => i.VacancyTasks)
           .FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken);
        _ = trainingVacancy ?? throw new NotFoundException(nameof(TrainingVacancy), request.Slug);

        _mapper.Map(request, trainingVacancy);
        trainingVacancy.Slug = _slugService.GenerateSlug($"{trainingVacancy.Title}-{trainingVacancy.PublishDate.Year}-{trainingVacancy.PublishDate.Month}");

        VacancyTimelineEvent timeLineEvent = new VacancyTimelineEvent
        {
            VacancyId = trainingVacancy.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Training Model updated",
            CreateBy = _currentUserService.GetUserName() ?? string.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timeLineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return trainingVacancy.Slug;
    }
}