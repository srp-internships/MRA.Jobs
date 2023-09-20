
namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Tags;

public class AddTagToTrainingVacancyCommandValidator : AbstractValidator<AddTagToTrainingVacancyCommand>
{
    public AddTagToTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.TrainingVacancySlug).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}