using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Tags;
public class AddTagToInternshipVacancyCommandHandler : IRequestHandler<AddTagToInternshipVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddTagToInternshipVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(AddTagToInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        var internship = await _context.InternshipVacancies
          .Include(x => x.Tags)
          .ThenInclude(t => t.Tag)
          .FirstOrDefaultAsync(x => x.Slug == request.InternshipSlug, cancellationToken);

        if (internship == null)
            throw new NotFoundException(nameof(internship), request.InternshipSlug);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tagName), cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            var vacancyTag = internship.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (vacancyTag == null)
            {
                vacancyTag = new VacancyTag { VacancyId = internship.Id, TagId = tag.Id };
                _context.VacancyTags.Add(vacancyTag);

                var timelineEvent = new VacancyTimelineEvent
                {
                    VacancyId = internship.Id,
                    EventType = TimelineEventType.Created,
                    Time = _dateTime.Now,
                    Note = $"Added '{tag.Name}' tag",
                    CreateBy = _currentUserService.GetId() ?? Guid.Empty
                };
                await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
