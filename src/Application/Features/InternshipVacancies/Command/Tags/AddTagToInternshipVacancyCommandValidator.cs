using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Tags;
public class AddTagToInternshipVacancyCommandValidator : AbstractValidator<AddTagToInternshipVacancyCommand>
{
    public AddTagToInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.InternshipId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
