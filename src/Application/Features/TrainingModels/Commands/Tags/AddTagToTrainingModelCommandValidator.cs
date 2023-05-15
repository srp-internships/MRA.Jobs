// Ignore Spelling: Validator

using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.Tags;
public class AddTagToTrainingModelCommandValidator : AbstractValidator<AddTagToTrainingModelCommand>
{
    public AddTagToTrainingModelCommandValidator()
    {
        RuleFor(x => x.TrainingModelId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
