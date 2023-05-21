using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.DeleteTrainingModel;
public class DeleteTrainingVacancyCommandHadler : IRequestHandler<DeleteTrainingVacancyCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTrainingVacancyCommandHadler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(DeleteTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var traningModel = await _context.TrainingModels.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

        if (traningModel == null)
            throw new NotFoundException(nameof(TrainingVacancy), request.Id);

        _context.TrainingModels.Remove(traningModel);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
