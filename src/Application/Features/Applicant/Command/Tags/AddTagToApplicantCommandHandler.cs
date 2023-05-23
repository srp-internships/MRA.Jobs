using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.CreateApplicant;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.Applicant.Command.Tags;
public class AddTagToApplicantCommandHandler : IRequestHandler<AddTagsToApplicantCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddTagToApplicantCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(AddTagsToApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(new object[] { request.ApplicantId }, cancellationToken);

        if (applicant == null)
            throw new NotFoundException(nameof(Applicant), request.ApplicantId);

        foreach (var tagName in request.Tags)
        {
            var tag = await _context.Tags.FindAsync(new object[] { tagName }, cancellationToken);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
            }

            var applicantTag = await _context.UserTags.FindAsync(new object[] { request.ApplicantId, tag.Id }, cancellationToken);

            if (applicantTag == null)
            {
                applicantTag = new UserTag { UserId = request.ApplicantId, TagId = tag.Id };
                _context.UserTags.Add(applicantTag);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public static implicit operator AddTagToApplicantCommandHandler(CreateApplicantCommandHandler v)
    {
        throw new NotImplementedException();
    }
}