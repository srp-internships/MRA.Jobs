﻿namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;

public class UpdateJobVacancyCommandValidator : AbstractValidator<UpdateJobVacancyCommand>
{
    public UpdateJobVacancyCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
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