using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;

namespace MRA.Jobs.Application.Features.Vacancies.Note.Commands;

public class ChangeVacancyNoteCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ChangeVacancyNoteCommand, bool>
{
    public async Task<bool> Handle(ChangeVacancyNoteCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await dbContext.Vacancies.FirstOrDefaultAsync(v => v.Id == request.VacancyId,
            cancellationToken);
        _ = vacancy ?? throw new NotFoundException(nameof(vacancy), request.VacancyId);

        vacancy.Note = request.Note;
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}