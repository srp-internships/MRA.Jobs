using System.Collections;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;

public class
    GetApplicationsPagedQueryHandler(IApplicationDbContext dbContext, IApplicationSieveProcessor sieveProcessor,
        IMapper mapper, ICurrentUserService currentUser)
    : IRequestHandler<PagedListQuery<ApplicationListDto>,
        PagedList<ApplicationListDto>>
{
    public Task<PagedList<ApplicationListDto>> Handle(PagedListQuery<ApplicationListDto> request, CancellationToken cancellationToken)
    {
        var allApplications = dbContext.Applications
            .AsNoTracking()
            .Include(a => a.Vacancy)
            .Include(a => a.VacancyResponses)
            .ThenInclude(vr => vr.VacancyQuestion);
        
        var roles = currentUser.GetRoles();
        var applications = roles.Count() == 1 && roles.Any(a => a == "Applicant")
            ? allApplications.Where(a => a.ApplicantUsername == currentUser.GetUserName() && a.Vacancy.Discriminator !="NoVacancy")
            : allApplications;

        var result = sieveProcessor.ApplyAdnGetPagedList(request, applications, mapper.Map<ApplicationListDto>);
        return Task.FromResult(result);
    }
}