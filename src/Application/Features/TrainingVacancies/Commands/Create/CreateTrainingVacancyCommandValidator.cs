using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Commands.Create;
public class CreateTrainingVacancyCommandValidator : AbstractValidator<CreateTrainingVacancyCommand>
{
    public CreateTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PublishDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Fees).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
    }
}
