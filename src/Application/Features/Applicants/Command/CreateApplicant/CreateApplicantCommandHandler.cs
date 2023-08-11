using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.CreateApplicant;

public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateApplicantCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Guid> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        Applicant applicant = _mapper.Map<Applicant>(request);
        await _context.Applicants.AddAsync(applicant, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return applicant.Id;
    }
}