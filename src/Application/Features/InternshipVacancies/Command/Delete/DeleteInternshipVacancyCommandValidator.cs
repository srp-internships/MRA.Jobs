using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Delete;

public class DeleteInternshipVacancyCommandValidator : AbstractValidator<DeleteInternshipVacancyCommand>
{
    public DeleteInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}