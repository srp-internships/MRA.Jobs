using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.DeleteJobVacancy;

public class DeleteJobVacancyCommandValidator : AbstractValidator<DeleteJobVacancyCommand>
{
    public DeleteJobVacancyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}