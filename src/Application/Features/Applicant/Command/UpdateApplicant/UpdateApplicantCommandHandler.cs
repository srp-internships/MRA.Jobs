using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.UpdateApplicant;

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
        var applicant =
            await _context.Applicants.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = applicant ?? throw new NotFoundException(nameof(Domain.Entities.Applicant), request.Id);
        
        _mapper.Map(request, applicant);
        
        await _context.SaveChangesAsync(cancellationToken);
        return applicant.Id;
    }
}