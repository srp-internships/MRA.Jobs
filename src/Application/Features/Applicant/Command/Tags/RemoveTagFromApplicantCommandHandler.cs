using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.Tags;
public class RemoveTagsFromApplicantCommandHandler : IRequestHandler<RemoveTagsFromApplicantCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RemoveTagsFromApplicantCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RemoveTagsFromApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(new object[] { request.ApplicantId }, cancellationToken);

        if (applicant == null)
            throw new NotFoundException(nameof(Applicant), request.ApplicantId);

        foreach (var tag in request.Tags)
        {
            var applicantTag = await _context.UserTags.FindAsync(new object[] {request.ApplicantId, tag }, cancellationToken);

            if (applicantTag != null)
                _context.UserTags.Remove(applicantTag);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
