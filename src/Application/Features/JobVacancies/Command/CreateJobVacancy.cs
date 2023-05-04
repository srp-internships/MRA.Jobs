using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Command;

using AutoMapper;
using Domain.Entities;

public class CreateJobVacancyCommandHadler : IRequestHandler<CreateJobVacancyCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateJobVacancyCommandHadler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<JobVacancy>(request);
        var result = _context.JobVacancies.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
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

        RuleFor(v => v.SalaryRange)
            .GreaterThan(0);

        RuleFor(e => e.Description)
            .NotEmpty()
            .MinimumLength(200);

        RuleFor(e => e.PublishDate)
            .Must((e, v) => v < e.EndDate).WithMessage("Publish date cannot be after end date");
    }
}


