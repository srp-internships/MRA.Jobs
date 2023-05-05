using AutoMapper;
using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.Internships.Command;
public class UpdateInternship : IRequestHandler<UpdateInternshipCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateInternship(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<long> Handle(UpdateInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Internship), request.Id);
        internship = _mapper.Map<Internship>(request);
        var result = _context.Internships.Update(internship);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }
}

public class UpdateInternshipCommandValidator : AbstractValidator<UpdateInternshipCommand>
{
    public UpdateInternshipCommandValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();

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