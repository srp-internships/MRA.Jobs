using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.DeleteInternship;
public class DeleteInternshipVacancyCommandValidator : AbstractValidator<DeleteInternshipVacancyCommand>
{
    public DeleteInternshipVacancyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
