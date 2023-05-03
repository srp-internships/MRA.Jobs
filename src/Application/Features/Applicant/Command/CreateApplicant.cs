using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command;
using MRA.Jobs.Domain.Entities;

public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateApplicantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<long> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        var entity = new Applicant()
        {
            Id = 1,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Patronymic = request.Patronymic,
            BirthDay = request.BirthDay,
            PhoneNumber = request.PhoneNumber
        };

        _context.Applicants.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public class ApplicantCommandValidator : AbstractValidator<CreateApplicantCommand>
{
    private readonly IApplicationDbContext _context;
    
    public ApplicantCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(v => v.LastName)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(v => v.Email)
            .NotEmpty()
            .EmailAddress(EmailValidationMode.AspNetCoreCompatible);

        RuleFor(v => v.Patronymic)
            .NotEmpty()
            .MinimumLength(3);
        
        RuleFor(v => v.PhoneNumber)
            .NotEmpty()
            .MinimumLength(9);
        

    }
}