// Ignore Spelling: Validator

using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.DeleteTrainingModel;
public class DeleteTrainingModelCommandValidator : AbstractValidator<DeleteTrainingVacancyCommand>
{
    public DeleteTrainingModelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
