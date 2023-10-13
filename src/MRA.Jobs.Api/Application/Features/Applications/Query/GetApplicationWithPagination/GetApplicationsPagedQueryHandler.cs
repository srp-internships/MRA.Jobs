using System.Collections;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;

public class
    GetApplicationsPagedQueryHandler : IRequestHandler<PagedListQuery<ApplicationListDto>,
        PagedList<ApplicationListDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetApplicationsPagedQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor,
        IMapper mapper, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public Task<PagedList<ApplicationListDto>> Handle(PagedListQuery<ApplicationListDto> request, CancellationToken cancellationToken)
    {
        var allApplications = _dbContext.Applications
            .AsNoTracking()
            .Include(a => a.Vacancy)
            .Include(a => a.VacancyResponses)
            .ThenInclude(vr => vr.VacancyQuestion);

        var applications = _currentUser.GetRoles().Any(a => a == "Applicant")
            ? allApplications.Where(a => a.ApplicantUsername == _currentUser.GetUserName())
            : allApplications;

        var result = _sieveProcessor.ApplyAdnGetPagedList(request, applications, _mapper.Map<ApplicationListDto>);
        return Task.FromResult(result);
    }
}