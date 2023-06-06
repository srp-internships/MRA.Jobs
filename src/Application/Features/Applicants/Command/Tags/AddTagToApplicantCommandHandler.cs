using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.Tags;
public class AddTagToApplicantCommandHandler : IRequestHandler<AddTagsToApplicantCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public AddTagToApplicantCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(AddTagsToApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants
         .Include(x => x.Tags)
         .ThenInclude(t => t.Tag)
         .FirstOrDefaultAsync(x => x.Id == request.ApplicantId, cancellationToken);

        if (applicant == null)
            throw new NotFoundException(nameof(Applicant), request.ApplicantId);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tagName), cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            var applicantTag = applicant.Tags.FirstOrDefault(t => t.Tag.Name == tagName);

            if (applicantTag == null)
            {
                applicantTag = new UserTag { UserId = request.ApplicantId, TagId = tag.Id };
                _context.UserTags.Add(applicantTag);

                var timelineEvent = new UserTimelineEvent
                {
                    UserId = applicant.Id,
                    EventType = TimelineEventType.Created,
                    Time = _dateTime.Now,
                    Note = $"Added '{tag.Name}' tag",
                    CreateBy = _currentUserService.GetId() ?? Guid.Empty
                };
                await _context.UserTimelineEvents.AddAsync(timelineEvent, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}