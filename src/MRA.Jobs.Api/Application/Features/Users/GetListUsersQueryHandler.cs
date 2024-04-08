using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MRA.Identity.Application.Contract.User.Commands.UsersByApplications;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Users;

namespace MRA.Jobs.Application.Features.Users;

public class GetListUsersQueryHandler(
    IConfiguration configuration,
    IHttpClientFactory clientFactory,
    ILogger<GetListUsersQuery> logger)
    : IRequestHandler<GetListUsersQuery, List<UserResponse>>
{
    public async Task<List<UserResponse>> Handle(GetListUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = clientFactory.CreateClient("mra-jobs");
            var command = new GetListUsersCommand()
            {
                ApplicationId = Guid.Parse(configuration["Application:Id"]),
                ApplicationClientSecret = configuration["Application:ClientSecret"],
                FullName = request.FullName,
                Skills = request.Skills,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
            };
            var response = await httpClient.PostAsJsonAsync(
                configuration["MraJobs-IdentityApi:Users"] + "GetListUsersCommand/ByFilter",
                command, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new NotFoundException("Application is not found");
                else
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<UserResponse>>(cancellationToken);
        }
        catch (Exception e)
        {
            logger.Log(LogLevel.Error, e.Message);
        }

        throw new Exception("server is not responding");
    }
}