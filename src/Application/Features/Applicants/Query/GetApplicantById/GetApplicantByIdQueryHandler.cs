using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;

namespace MRA.Jobs.Application.Features.Applicants.Query.GetApplicantById;

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
        Applicant applicant = await _context.Applicants
            .Include(a => a.History)
            .Include(a => a.SocialMedias)
            .Include(a => a.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        _ = applicant ?? throw new NotFoundException(nameof(Applicant), request.Id);
        return _mapper.Map<ApplicantDetailsDto>(applicant);
    }
}