namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;

public class DeleteTrainingVacancyCommandValidator : AbstractValidator<DeleteTrainingVacancyCommand>
{
    public DeleteTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}