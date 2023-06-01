using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancyTest;
public class CreateJobVacancyTestCommandValidator : AbstractValidator<CreateJobVacancyTestCommand>
{
    public CreateJobVacancyTestCommandValidator()
    {
        RuleFor(x => x.NumberOfQuestion).NotEmpty();
        RuleFor(x => x.Categories).NotEmpty();
        RuleForEach(x => x.Categories).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}
