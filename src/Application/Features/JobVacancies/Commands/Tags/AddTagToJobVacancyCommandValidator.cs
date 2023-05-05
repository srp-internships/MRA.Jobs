using FluentValidation;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

public class AddTagToJobVacancyCommandValidator : AbstractValidator<AddTagToJobVacancyCommand>
{
    public AddTagToJobVacancyCommandValidator()
    {
        RuleFor(x => x.JobVacancyId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
