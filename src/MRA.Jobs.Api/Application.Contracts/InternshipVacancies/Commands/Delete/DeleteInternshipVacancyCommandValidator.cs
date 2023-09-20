
namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;

public class DeleteInternshipVacancyCommandValidator : AbstractValidator<DeleteInternshipVacancyCommand>
{
    public DeleteInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}