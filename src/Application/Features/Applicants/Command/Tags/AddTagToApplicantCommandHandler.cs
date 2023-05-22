using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.Tags;
public class AddTagToApplicantCommandHandler : IRequestHandler<AddTagToApplicantCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public AddTagToApplicantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(AddTagToApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _dbContext.Applicants.FindAsync(new object[] { request.ApplicantId }, cancellationToken);
        var tag = await _dbContext.Tags.FindAsync(new object[] { request.TagId }, cancellationToken: cancellationToken);

        var userTag = new UserTag
        {
            UserId = applicant?.Id ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId),
            TagId = tag?.Id ?? throw new NotFoundException(nameof(Tag), request.TagId)
        };

        await _dbContext.UserTags.AddAsync(userTag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
