// Ignore Spelling: Validator

using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Delete;
public class DeleteTrainingVacancyCommandValidator : AbstractValidator<DeleteTrainingVacancyCommand>
{
    public DeleteTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}
