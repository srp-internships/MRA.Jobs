using MediatR;
using AutoMapper;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications.Command;
using MRA.Jobs.Domain.Entities;

public class UpdateApplicationCommandHadler : IRequestHandler<UpdateApplicationCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateApplicationCommandHadler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Applications.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Application), request.Id);

        var applicant = await _context.Applicants.FindAsync(request.ApplicantId, cancellationToken)
             ?? throw new NotFoundException(nameof(Applicant), request.ApplicantId);
        var vacancy = await _context.Vacancies.FindAsync(request.VacancyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);

        var application = _mapper.Map<Application>(request);

        _context.Applications.Update(application);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
