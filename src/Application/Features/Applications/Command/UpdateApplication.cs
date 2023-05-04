using MediatR;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command;
using MRA.Jobs.Domain.Entities;
public class UpdateApplicationCommandHadler : IRequestHandler<UpdateApplicationCommand, long>
{
    private readonly IApplicationDbContext _context;

    public UpdateApplicationCommandHadler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Applications.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Application), request.Id);

        var applicant = await _context.Applicants.FindAsync(request.ApplicantId, cancellationToken);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId, cancellationToken);

        entity.Applicant = applicant ?? throw new NotFoundException(nameof(Applicant), request.Id);
        entity.Vacancy = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.Id);
        entity.CoverLetter = request.CoverLetter;
        entity.History = request.History;
        entity.ResumeUrl = request.ResumeUrl;

        _context.Applications.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
