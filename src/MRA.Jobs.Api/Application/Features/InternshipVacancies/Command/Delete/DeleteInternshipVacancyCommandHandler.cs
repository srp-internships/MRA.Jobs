﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;

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
        var internship = await _context.Internships.FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken);

        if (internship == null)
            throw new NotFoundException(nameof(InternshipVacancy), request.Slug);

        _context.Internships.Remove(internship);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
