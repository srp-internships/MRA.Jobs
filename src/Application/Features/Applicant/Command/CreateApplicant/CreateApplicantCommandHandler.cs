using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.CreateApplicant;

public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dataTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateApplicantCommandHandler(
        IMapper mapper, 
        IApplicationDbContext context,
        IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _context = context;
        _dataTime = dateTime;
        _currentUserService = currentUserService;
    }
    public async Task<Guid> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId);
        _ = applicant ?? throw new NotFoundException(nameof(Domain.Entities.Applicant), request);

        var user = _mapper.Map<Domain.Entities.Applicant>(request);
        await _context.Applicants.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}