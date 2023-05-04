using MediatR;
using FluentValidation;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command;
using MRA.Jobs.Domain.Entities;
public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateApplicationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<long> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId, cancellationToken);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId, cancellationToken);

        var entity = new Application()
        {
           Applicant = applicant ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId),
           Vacancy = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId),
           CoverLetter = request.CoverLetter,
           History = request.History,
           ResumeUrl = request.ResumeUrl,
           StatusId = (Domain.Enums.ApplicationStatus)request.StatusId,
        };

        _context.Applications.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public class CreateApplicationCommandValidator: AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(v => v.CoverLetter)
            .NotEmpty()
            .MinimumLength(150);
        RuleFor(v => v.ResumeUrl)
            .NotEmpty();
    }
}
