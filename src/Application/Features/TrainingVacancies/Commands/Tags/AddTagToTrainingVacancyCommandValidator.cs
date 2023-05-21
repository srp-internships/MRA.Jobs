// Ignore Spelling: Validator

using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;
public class AddTagToTrainingVacancyCommandValidator : AbstractValidator<AddTagToTrainingVacancyCommand>
{
    public AddTagToTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.TrainingModelId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
