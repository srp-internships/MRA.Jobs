using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Delete;

public class DeleteTrainingVacancyCommandHadler : IRequestHandler<DeleteTrainingVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTrainingVacancyCommandHadler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var traningModel = await _context.TrainingVacancies.FirstOrDefaultAsync(t => t.Slug == request.Slug, cancellationToken);

        if (traningModel == null)
            throw new NotFoundException(nameof(TrainingVacancy), request.Slug);

        _context.TrainingVacancies.Remove(traningModel);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}