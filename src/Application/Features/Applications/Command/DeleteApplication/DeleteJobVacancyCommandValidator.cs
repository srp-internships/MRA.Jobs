using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command.DeleteApplication;

public class DeleteApplicationCommandValidator : AbstractValidator<DeleteJobVacancyCommand>
{
    public DeleteApplicationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}