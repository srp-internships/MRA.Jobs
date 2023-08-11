using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Tags;

public class RemoveTagFromTrainingVacancyCommandValidator : AbstractValidator<RemoveTagFromTrainingVacancyCommand>
{
    public RemoveTagFromTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.VacancyId).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}