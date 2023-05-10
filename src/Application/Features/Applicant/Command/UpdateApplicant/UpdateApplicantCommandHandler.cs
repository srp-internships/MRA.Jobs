using System.Runtime.InteropServices.ComTypes;
using MRA.Jobs.Application.Contracts.Applicant.Commands;

namespace MRA.Jobs.Application.Features.Applicant.Command.UpdateApplicant;

public class UpdateApplicantCommandHandler : IRequestHandler<UpdateApplicantCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public UpdateApplicantCommandHandler(IApplicationDbContext context,
        IMapper mapper,
        IDateTime dateTime, 
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
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