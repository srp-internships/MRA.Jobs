using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.JobVacancies.Command;

public class CreateJobVacancyCommandHadler : IRequestHandler<CreateJobVacancyCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateJobVacancyCommandHadler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var entity = new JobVacancy()
        {
            Title = request.Title,
            Category = _context.Categories.Find(request.CategoryId),
            Description = request.Description,
            EndDate = request.EndDate,
            PublishDate = request.PublishDate,
            ShortDescription = request.ShortDescription,
            RequiredYearOfExperience = request.RequiredYearOfExperience,
            WorkSchedule = (WorkSchedule)request.WorkSchedule
        };

        _context.JobVacancies.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;

    }
}

public class CreateJobVacancyCommandValidator : AbstractValidator<CreateJobVacancyCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateJobVacancyCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(15)
            .MaximumLength(50);

        RuleFor(v => v.Salary)
            .GreaterThan(0);

        RuleFor(e => e.Description)
            .NotEmpty()
            .MinimumLength(200);

        RuleFor(e => e.PublishDate)
            .Must((e, v) => v < e.EndDate).WithMessage("Publish date cannot be after end date");
    }
}


