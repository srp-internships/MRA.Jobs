using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.DeleteInternship;
public class DeleteInternshipCommandHandler : IRequestHandler<DeleteInternshipCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteInternshipCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken);

        if (internship == null)
            throw new NotFoundException(nameof(InternshipVacancy), request.Id);

        _context.Internships.Remove(internship);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
