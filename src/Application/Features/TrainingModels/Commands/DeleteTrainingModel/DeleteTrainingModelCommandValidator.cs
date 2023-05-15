// Ignore Spelling: Validator

using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.DeleteTraningModel;
public class DeleteTrainingModelCommandValidator : AbstractValidator<DeleteTrainingModelCommand>
{
    public DeleteTrainingModelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
