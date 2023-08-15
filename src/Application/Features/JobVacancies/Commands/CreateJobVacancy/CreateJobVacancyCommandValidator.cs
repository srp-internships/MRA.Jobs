using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancy;

public class CreateJobVacancyCommandValidator : AbstractValidator<CreateJobVacancyCommand>
{
    public CreateJobVacancyCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PublishDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.RequiredYearOfExperience).GreaterThanOrEqualTo(0);
        RuleFor(x => x.WorkSchedule).IsInEnum();
    }
}