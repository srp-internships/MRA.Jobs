using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;

public class GetApplicationsByFiltersQueryHandler(
    IApplicationDbContext dbContext,
    IHttpClientFactory clientFactory,
    ICurrentUserService currentUser,
    IConfiguration configuration,
    IApplicationSieveProcessor sieveProcessor,
    IMapper mapper,
    IUsersService usersService)
    : IRequestHandler<GetApplicationsByFiltersQuery, PagedList<ApplicationListDto>>
{
    public async Task<PagedList<ApplicationListDto>> Handle(GetApplicationsByFiltersQuery request,
        CancellationToken cancellationToken)
    {
        var applications = dbContext.Applications
            .Include(a => a.Vacancy)
            .Include(a => a.VacancyResponses)
            .AsNoTracking();

        var users =
            await usersService.GetUsersAsync(request.Filters, request.Email, request.PhoneNumber, request.Skills);

        if (!request.FullName.IsNullOrEmpty() || !request.Skills.IsNullOrEmpty() ||
            !request.PhoneNumber.IsNullOrEmpty() || !request.Email.IsNullOrEmpty())
        {
            applications = applications.Where(a => users.Select(u => u.UserName).Contains(a.ApplicantUsername));
        }

        var roles = currentUser.GetRoles();
        if (roles.All(r => r != "Reviewer"))
            applications = applications.Where(a =>
                a.ApplicantUsername == currentUser.GetUserName() && a.Vacancy.Discriminator != "NoVacancy");

        var result = sieveProcessor.ApplyAdnGetPagedList(request, applications, mapper.Map<ApplicationListDto>);
        result.Items.ForEach(application =>
            application.User = users.FirstOrDefault(user => user.UserName == application.ApplicantUsername));

        return result;
    }
}