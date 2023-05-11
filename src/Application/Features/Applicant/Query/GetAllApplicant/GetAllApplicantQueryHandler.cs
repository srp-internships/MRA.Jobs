using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;

namespace MRA.Jobs.Application.Features.Applicant.Query.GetAllApplicant;

public class GetAllApplicantQueryHandler : IRequestHandler<GetAllApplicantQuery, ApplicantDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetAllApplicantQueryHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ApplicantDetailsDto> Handle(GetAllApplicantQuery request, CancellationToken cancellationToken)
    {
        var applicants =
            await _context.Applicants.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = applicants ?? throw new NotFoundException(nameof(Domain.Entities.Applicant), request.Id);
        return _mapper.Map<ApplicantDetailsDto>(applicants);
    }
}