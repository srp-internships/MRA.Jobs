// Ignore Spelling: Validator

using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.UpdateTraningModel;
public class UpdateTrainingModelCommandValidator : AbstractValidator<UpdateTrainingModelCommand>
{
    public UpdateTrainingModelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PublishDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Fees).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
    }
}
