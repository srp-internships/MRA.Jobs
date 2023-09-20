
namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
public class UpdateTrainingVacancyCommandValidator : AbstractValidator<UpdateTrainingVacancyCommand>
{
    public UpdateTrainingVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
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