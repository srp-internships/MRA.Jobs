using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;
public class RemoveTagFromTrainingVacancyCommandValidator : AbstractValidator<RemoveTagFromTrainingVacancyCommand>
{
    public RemoveTagFromTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.TrainingModelId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
