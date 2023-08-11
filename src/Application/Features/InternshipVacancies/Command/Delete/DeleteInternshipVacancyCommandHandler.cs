using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Delete;

public class DeleteInternshipVacancyCommandHandler : IRequestHandler<DeleteInternshipVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteInternshipVacancyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        //var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken);
        InternshipVacancy internship =
            await _context.Internships.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (internship == null)
        {
            throw new NotFoundException(nameof(InternshipVacancy), request.Id);
        }

        _context.Internships.Remove(internship);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}