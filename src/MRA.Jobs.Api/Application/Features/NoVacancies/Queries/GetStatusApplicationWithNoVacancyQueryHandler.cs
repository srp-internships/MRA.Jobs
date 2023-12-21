// using Microsoft.AspNetCore.Http;
// using Microsoft.EntityFrameworkCore;
// using MRA.Jobs.Application.Contracts.Dtos.Enums;
// using MRA.Jobs.Application.Contracts.NoVacancies.Queries.GetApplicationWithNoVacancy;
// using MRA.Jobs.Application.Contracts.NoVacancies.Responses;
//
// namespace MRA.Jobs.Application.Features.NoVacancies.Queries;
//
// public class GetStatusApplicationWithNoVacancyQueryHandler(IApplicationDbContext dbContext,
//     ICurrentUserService userService) : IRequestHandler<GetStatusApplicationWithNoVacancyQuery,
//     ApplicationWithNoVacancyStatus>
// {
//     public async Task<ApplicationWithNoVacancyStatus> Handle(GetStatusApplicationWithNoVacancyQuery request,
//         CancellationToken cancellationToken)
//     {
//         var userName = userService.GetUserName();
//         var application = await dbContext.Applications
//             .Where(a => a.Slug.Contains($"{userName}-no_vacancy"))
//             .OrderByDescending(a => a.AppliedAt)
//             .FirstOrDefaultAsync(cancellationToken);
//         if (application == null)
//             return new ApplicationWithNoVacancyStatus() { Applied = false, Status = null };
//
//         return new ApplicationWithNoVacancyStatus()
//         {
//             Status = (ApplicationStatusDto.ApplicationStatus)application.Status
//         };
//     }
// }