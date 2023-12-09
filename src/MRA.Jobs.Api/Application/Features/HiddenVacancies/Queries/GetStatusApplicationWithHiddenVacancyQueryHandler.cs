using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Queries.GetApplicationWithHiddenVacancy;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;

namespace MRA.Jobs.Application.Features.HiddenVacancies.Queries;

public class GetStatusApplicationWithHiddenVacancyQueryHandler(IApplicationDbContext dbContext,
    ICurrentUserService userService) : IRequestHandler<GetStatusApplicationWithHiddenVacancyQuery,
    ApplicationWithHiddenVacancyStatus>
{
    public async Task<ApplicationWithHiddenVacancyStatus> Handle(GetStatusApplicationWithHiddenVacancyQuery request,
        CancellationToken cancellationToken)
    {
        var userName = userService.GetUserName();
        var application = await dbContext.Applications
            .Where(a => a.Slug.Contains($"{userName}-hidden_vacancy"))
            .OrderByDescending(a => a.AppliedAt)
            .FirstOrDefaultAsync(cancellationToken);
        if (application == null)
            return new ApplicationWithHiddenVacancyStatus() { Applied = false, Status = null };

        return new ApplicationWithHiddenVacancyStatus()
        {
            Status = (ApplicationStatusDto.ApplicationStatus)application.Status
        };
    }
}