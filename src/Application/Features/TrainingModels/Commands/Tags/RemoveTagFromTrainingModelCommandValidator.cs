using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TrainingModels.Commands.Tags;
public class RemoveTagFromTrainingModelCommandValidator : AbstractValidator<RemoveTagFromTrainingModelCommand>
{
    public RemoveTagFromTrainingModelCommandValidator()
    {
        RuleFor(x => x.TrainingModelId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}
