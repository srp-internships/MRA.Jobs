using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MRA.Identity.Application.Contract.User.Commands.UsersByApplications;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Users;

namespace MRA.Jobs.Application.Features.Users;

public class GetPagedListUsersQueryHandler(
    IConfiguration configuration,
    IHttpClientFactory clientFactory,
    ILogger<GetPagedListUsersQuery> logger)
    : IRequestHandler<GetPagedListUsersQuery, PagedList<UserResponse>>
{
    public async Task<PagedList<UserResponse>> Handle(GetPagedListUsersQuery request,
        CancellationToken cancellationToken)
    {
        var httpClient = clientFactory.CreateClient("mra-jobs");
        var command = new GetPagedListUsersCommand()
        {
            ApplicationId = Guid.Parse(configuration["Application:Id"]),
            ApplicationClientSecret = configuration["Application:ClientSecret"],
            Filters = request.Filters,
            Skills = request.Skills,
            Sorts = request.Sorts,
            PageSize = request.PageSize,
            Page = request.Page
        };
        var response = await httpClient.PostAsJsonAsync(
            configuration["MraJobs-IdentityApi:Users"],
            command, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                logger.Log(LogLevel.Error, "Application is not found");
                throw new NotFoundException("Application is not found");
            }
            else
            {
                logger.Log(LogLevel.Error, $"Request failed with status code {response.StatusCode}");
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }

        return await response.Content.ReadFromJsonAsync<PagedList<UserResponse>>(cancellationToken);
    }
}