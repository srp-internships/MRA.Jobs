using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Application.Features.Internships.Command.Tags;
public class RemoveTagFromInternshipCommandHandler : IRequestHandler<RemoveTagFromInternshipCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public RemoveTagFromInternshipCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(RemoveTagFromInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships.FindAsync(new object[] { request.InternshipId }, cancellationToken);

        if (internship == null)
            throw new NotFoundException(nameof(internship), request.InternshipId);

        foreach (var tag in request.Tags)
        {
            var internshipTag = internship.Tags.FirstOrDefault(t => t.Tag.Name == tag);

            if (internshipTag != null)
                _context.VacancyTags.Remove(internshipTag);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
