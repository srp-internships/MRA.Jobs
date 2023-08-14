using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicants.Command.UpdateApplicant;

public class UpdateApplicantCommandHandler : IRequestHandler<UpdateApplicantCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateApplicantCommandHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateApplicantCommand request, CancellationToken cancellationToken)
    {
        Applicant applicant =
            await _context.Applicants.FindAsync(new object[] { request.Id }, cancellationToken);
        _ = applicant ?? throw new NotFoundException(nameof(Applicant), request.Id);

        _mapper.Map(request, applicant);

        await _context.SaveChangesAsync(cancellationToken);
        return applicant.Id;
    }
}