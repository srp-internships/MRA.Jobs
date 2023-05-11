using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;

namespace MRA.Jobs.Application.Features.Applicant.Query.GetApplicantById;

public class GetApplicantQueryHandler : IRequestHandler<GetApplicantByIdQuery, ApplicantDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetApplicantQueryHandler(
        IApplicationDbContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApplicantDetailsDto> Handle(GetApplicantByIdQuery request, CancellationToken cancellationToken)
    {
        var applicant =
            await _context.Applicants.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = applicant ?? throw new NotFoundException(nameof(Domain.Entities.Applicant), request.Id);
        return _mapper.Map<ApplicantDetailsDto>(applicant);
    }
}