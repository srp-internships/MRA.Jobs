using AutoMapper;
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
    private readonly IMapper _mapper;

    public UpdateJobVacancyCommandHadler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.JobVacancies.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(JobVacancy), request.Id);
        entity = _mapper.Map<JobVacancy>(request);
        var result = _context.JobVacancies.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
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

        RuleFor(v => v.SalaryRange)
            .GreaterThan(0);

        RuleFor(e => e.Description)
            .NotEmpty()
            .MinimumLength(200);

        RuleFor(e => e.PublishDate)
            .Must((e, v) => v < e.EndDate).WithMessage("Publish date cannot be after end date");
    }
}


