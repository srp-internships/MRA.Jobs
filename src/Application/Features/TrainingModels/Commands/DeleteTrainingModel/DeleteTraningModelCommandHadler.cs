using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.DeleteTraningModel;
public class DeleteTraningModelCommandHadler : IRequestHandler<DeleteTrainingModelCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTraningModelCommandHadler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(DeleteTrainingModelCommand request, CancellationToken cancellationToken)
    {
        var traningModel = await _context.TrainingModels.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

        if (traningModel == null)
            throw new NotFoundException(nameof(TrainingModel), request.Id);

        _context.TrainingModels.Remove(traningModel);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
