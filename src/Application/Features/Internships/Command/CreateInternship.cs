using AutoMapper;
using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.Internships.Command;
public class CreateInternship : IRequestHandler<CreateInternshipCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateInternship(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<long> Handle(CreateInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = _mapper.Map<Internship>(request);
        var result = _context.Internships.Add(internship);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }
}

public class CreateInternshipCommandValidator : AbstractValidator<CreateInternshipCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateInternshipCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(15)
            .MaximumLength(50);

        RuleFor(e => e.Description)
            .NotEmpty()
            .MinimumLength(200);

        RuleFor(e => e.PublishDate)
            .Must((e, v) => v < e.EndDate).WithMessage("Publish date cannot be after end date");
    }
}