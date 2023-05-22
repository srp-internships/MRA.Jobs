﻿using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.UpdateTrainingModel;
public class UpdateTrainingVacancyCommandHandler : IRequestHandler<UpdateTrainingVacancyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public UpdateTrainingVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }
    public async Task<Guid> Handle(UpdateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var trainingModel = await _context.TrainingModels.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = trainingModel ?? throw new NotFoundException(nameof(TrainingVacancy), request.Id);

        var category = await _context.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken: cancellationToken);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        _mapper.Map(request, trainingModel);

        var timeLineEvent = new VacancyTimelineEvent
        {
            VacancyId = trainingModel.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Training Model updated",
            CreateBy = _currentUserService.GetId().Value
        };
        await _context.VacancyTimelineEvents.AddAsync(timeLineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return trainingModel.Id;
    }
}
