using FluentValidation;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

public class RemoveTagFromJobVacancyCommandValidator : AbstractValidator<RemoveTagFromJobVacancyCommand>
{
    public RemoveTagFromJobVacancyCommandValidator()
    {
        RuleFor(x => x.JobVacancyId).NotEmpty();
        RuleFor(x => x.TagId).NotEmpty();
    }
}
