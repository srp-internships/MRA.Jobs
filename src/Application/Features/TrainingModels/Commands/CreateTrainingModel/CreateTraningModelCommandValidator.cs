using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TraningModels.Commands.CreateTraningModel;
public class CreateTraningModelCommandValidator : AbstractValidator<CreateTrainingModelCommand>
{
    public CreateTraningModelCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PublishDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Fees).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Duration).GreaterThanOrEqualTo(0);
    }
}
