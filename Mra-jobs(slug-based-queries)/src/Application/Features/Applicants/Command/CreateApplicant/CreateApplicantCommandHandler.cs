using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.CreateApplicant;
using MRA.Jobs.Domain.Entities;

public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public CreateApplicantCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Guid> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = _mapper.Map<Applicant>(request);
        await _context.Applicants.AddAsync(applicant, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return applicant.Id;
    }
}