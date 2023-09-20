namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands.DeleteJobVacancy;

public class DeleteJobVacancyCommandValidator : AbstractValidator<DeleteJobVacancyCommand>
{
    public DeleteJobVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}