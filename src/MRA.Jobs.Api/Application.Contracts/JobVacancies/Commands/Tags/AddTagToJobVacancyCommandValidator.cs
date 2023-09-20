
namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands.Tags;


public class AddTagToJobVacancyCommandValidator : AbstractValidator<AddTagsToJobVacancyCommand>
{
    public AddTagToJobVacancyCommandValidator()
    {
        RuleFor(x => x.JobVacancySlug).NotEmpty();
        RuleFor(x => x.Tags).NotEmpty();
    }
}