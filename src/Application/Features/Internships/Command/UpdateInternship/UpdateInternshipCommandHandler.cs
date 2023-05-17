using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.Internships.Command.UpdateInternship;
public class UpdateInternshipCommandHandler : IRequestHandler<UpdateInternshipCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public UpdateInternshipCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = internship ?? throw new NotFoundException(nameof(Internship), request.Id);

        var category = await _context.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken: cancellationToken);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        _mapper.Map(request, internship);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = internship.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Internship updated",
            CreateBy = _currentUserService.UserId
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return internship.Id;
    }
}