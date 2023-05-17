using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.DeleteApplicant;

public class DeleteApplicantCommandHandler : IRequestHandler<DeleteApplicantCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteApplicantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

        if (applicant == null)
            throw new NotFoundException(nameof(Domain.Entities.Applicant), request.Id);

        _context.Applicants.Remove(applicant);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}