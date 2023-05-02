using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.JobVacancies.Command;

public class UpdateJobVacancyCommandHadler : IRequestHandler<UpdateJobVacancyCommand, long>
{
    private readonly IApplicationDbContext _context;

    public UpdateJobVacancyCommandHadler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.JobVacancies.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(JobVacancy), request.Id);

        var category = await _context.Categories.FindAsync(request.CategoryId, cancellationToken);
        entity.Title = request.Title;
        entity.Category = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);
        entity.Description = request.Description;
        entity.EndDate = request.EndDate;
        entity.PublishDate = request.PublishDate;
        entity.ShortDescription = request.ShortDescription;
        entity.RequiredYearOfExperience = request.RequiredYearOfExperience;
        entity.WorkSchedule = (WorkSchedule)request.WorkSchedule;

        _context.JobVacancies.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;

    }
}

public class UpdateJobVacancyCommandValidator : AbstractValidator<UpdateJobVacancyCommand>
{
    public UpdateJobVacancyCommandValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();

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


