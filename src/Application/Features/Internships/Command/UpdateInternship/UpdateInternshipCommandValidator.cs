using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.Internships.Command.UpdateInternship;
public class UpdateInternshipCommandValidator : AbstractValidator<UpdateInternshipCommand>
{
    public UpdateInternshipCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
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