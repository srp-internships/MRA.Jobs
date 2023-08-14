using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.DeleteApplicant;

public class DeleteApplicantCommandHandler : IRequestHandler<DeleteApplicantCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteApplicantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteApplicantCommand request, CancellationToken cancellationToken)
    {
        Applicant applicant = await _context.Applicants.FindAsync(new object[] { request.Id }, cancellationToken);

        if (applicant == null)
        {
            throw new NotFoundException(nameof(Applicant), request.Id);
        }

        _context.Applicants.Remove(applicant);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}