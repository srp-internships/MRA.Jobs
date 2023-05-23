// Ignore Spelling: Validator

using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;
public class AddTagToTrainingVacancyCommandValidator : AbstractValidator<AddTagToTrainingVacancyCommand>
{
    public AddTagToTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.VacancyId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}
