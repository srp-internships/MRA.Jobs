namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands.Tags;


public class RemoveTagFromJobVacancyCommandValidator : AbstractValidator<RemoveTagsFromJobVacancyCommand>
{
    public RemoveTagFromJobVacancyCommandValidator()
    {
        RuleFor(x => x.JobVacancySlug).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}