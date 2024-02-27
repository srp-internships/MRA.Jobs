using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
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
    IMapper mapper)
    : IRequestHandler<GetApplicationsByFiltersQuery, PagedList<ApplicationListDto>>
{
    public async Task<PagedList<ApplicationListDto>> Handle(GetApplicationsByFiltersQuery request,
        CancellationToken cancellationToken)
    {
        var applications = dbContext.Applications
            .Include(a => a.Vacancy)
            .AsNoTracking();

        List<UserResponse> users = new();

        var token = currentUser.GetAuthToken();
        if (!token.IsNullOrEmpty())
        {
            var httpClient = clientFactory.CreateClient("Mra.Identity");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

            var queryParameters = new Dictionary<string, string>();
            if (!request.FullName.IsNullOrEmpty()) queryParameters.Add("FullName", request.FullName.Trim());
            if (!request.PhoneNumber.IsNullOrEmpty()) queryParameters.Add("PhoneNumber", request.PhoneNumber.Trim());
            if (!request.Email.IsNullOrEmpty()) queryParameters.Add("Email", request.Email.Trim());
            if (!request.Skills.IsNullOrEmpty()) queryParameters.Add("Skills", request.Skills.Trim());
            
            var queryString = QueryHelpers.AddQueryString(configuration["MraJobs-IdentityApi:Users"], queryParameters);
            try
            {
                users = await httpClient.GetFromJsonAsync<List<UserResponse>>(queryString, cancellationToken);

                if (queryParameters.Any())
                {
                    applications = applications.Where(a => users.Select(u => u.UserName).Contains(a.ApplicantUsername));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        var result = sieveProcessor.ApplyAdnGetPagedList(request, applications, mapper.Map<ApplicationListDto>);
        result.Items.ForEach(application =>
            application.User = users.FirstOrDefault(user => user.UserName == application.ApplicantUsername));

        return result;
    }
}