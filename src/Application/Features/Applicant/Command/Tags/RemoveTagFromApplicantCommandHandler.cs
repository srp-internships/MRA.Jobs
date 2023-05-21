using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.Tags;
public class RemoveTagFromApplicantCommandHandler : IRequestHandler<RemoveTagFromApplicantCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public RemoveTagFromApplicantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(RemoveTagFromApplicantCommand request, CancellationToken cancellationToken)
    {
        var userTag = await _dbContext.UserTags
          .FirstOrDefaultAsync(vt => vt.UserId == request.ApplicantId && vt.TagId == request.TagId, cancellationToken);

        _ = userTag ?? throw new NotFoundException(nameof(VacancyTag), request.TagId);

        _ = _dbContext.UserTags.Remove(userTag);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
