using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Update;
public class UpdateInternshipVacancyCommandValidator : AbstractValidator<UpdateInternshipVacancyCommand>
{
    public UpdateInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PublishDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ApplicationDeadline).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
        RuleFor(x => x.Stipend).NotEmpty();
    }
}