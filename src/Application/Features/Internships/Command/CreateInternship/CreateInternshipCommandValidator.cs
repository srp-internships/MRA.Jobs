﻿using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.Internships.Command.CreateInternship;
public class CreateInternshipCommandValidator : AbstractValidator<CreateInternshipCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateInternshipCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PublishDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ApplicationDeadline).NotEmpty();
        RuleFor(x => x.Duration).NotEmpty();
        RuleFor(x => x.Stipend).NotEmpty();
    }
}
