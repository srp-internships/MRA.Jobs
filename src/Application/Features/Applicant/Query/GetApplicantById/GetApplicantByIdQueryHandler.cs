using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;

namespace MRA.Jobs.Application.Features.Applicant.Query.GetApplicantById;
using Domain.Entities;
public class GetApplicantByIdQueryHandler : IRequestHandler<GetApplicantByIdQuery, ApplicantDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetApplicantByIdQueryHandler(
        IApplicationDbContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApplicantDetailsDto> Handle(GetApplicantByIdQuery request, CancellationToken cancellationToken)
    {
        var applicant = await _context.Applicants.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        _ = applicant ?? throw new NotFoundException(nameof(Applicant), request.Id);
        return _mapper.Map<ApplicantDetailsDto>(applicant);
    }
}