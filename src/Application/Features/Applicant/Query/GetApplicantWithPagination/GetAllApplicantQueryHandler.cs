using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Infrastructure;

namespace MRA.Jobs.Application.Features.Applicant.Query.GetApplicantWithPagination;

public class GetAllApplicantQueryHandler : IRequestHandler<PaggedListQuery<ApplicantListDto>, PaggedList<ApplicantListDto>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IApplicationSieveProcessor _applicationSieveProcessor;

    public GetAllApplicantQueryHandler(IApplicationDbContext context,
        IMapper mapper, 
        IApplicationSieveProcessor applicationSieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _applicationSieveProcessor = applicationSieveProcessor;
    }
    
    public async Task<PaggedList<ApplicantListDto>> Handle(PaggedListQuery<ApplicantListDto> request, CancellationToken cancellationToken)
    {
        var applicants =_applicationSieveProcessor.ApplyAdnGetPaggedList(request, _context.Applicants.AsNoTracking(), _mapper.Map<ApplicantListDto>);
        return await Task.FromResult(applicants);;
    }
}