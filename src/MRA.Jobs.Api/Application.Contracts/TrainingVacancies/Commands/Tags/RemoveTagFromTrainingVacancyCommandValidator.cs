namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Tags;

public class RemoveTagFromTrainingVacancyCommandValidator : AbstractValidator<RemoveTagFromTrainingVacancyCommand>
{
    public RemoveTagFromTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.TrainingVacancySlug).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}