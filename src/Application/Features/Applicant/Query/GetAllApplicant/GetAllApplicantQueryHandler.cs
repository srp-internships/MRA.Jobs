using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Contracts.Applicant.Responses;

namespace MRA.Jobs.Application.Features.Applicant.Query.GetAllApplicant;

public class GetAllApplicantQueryHandler : IRequestHandler<GetAllApplicantQuery, List<ApplicantListDto>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetAllApplicantQueryHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<ApplicantListDto>> Handle(GetAllApplicantQuery request, CancellationToken cancellationToken)
    {
        var applicants =
            await _context.Applicants.ToListAsync();
        
        return _mapper.Map<List<ApplicantListDto>>(applicants);
    }
}