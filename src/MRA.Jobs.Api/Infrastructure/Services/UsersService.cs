using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Applications.Candidates;

namespace MRA.Jobs.Infrastructure.Services;

public class UsersService(
    ICurrentUserService currentUserService,
    IHttpClientFactory clientFactory,
    IConfiguration configuration,
    ILogger<UsersService> logger) : IUsersService
{
    public async Task<List<UserResponse>> GetUsersAsync(GetCandidatesQuery query)
    {
        List<UserResponse> users = new();
        var token = currentUserService.GetAuthToken();
        if (token.IsNullOrEmpty())
        {
            return users;
        }

        var httpClient = clientFactory.CreateClient("Mra.Identity");
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

        var queryParameters = new Dictionary<string, string>();
        if (!query.FullName.IsNullOrEmpty()) queryParameters.Add("FullName", query.FullName.Trim());
        if (!query.PhoneNumber.IsNullOrEmpty()) queryParameters.Add("PhoneNumber", query.PhoneNumber.Trim());
        if (!query.Email.IsNullOrEmpty()) queryParameters.Add("Email", query.Email.Trim());
        if (!query.Skills.IsNullOrEmpty()) queryParameters.Add("Skills", query.Skills.Trim());

        var queryString = QueryHelpers.AddQueryString(configuration["MraJobs-IdentityApi:Users"], queryParameters);
        try
        {
            users = await httpClient.GetFromJsonAsync<List<UserResponse>>(queryString);
        }
        catch (Exception e)
        {
            logger.LogError(e,e.Message);
        }

        return users;
    }
}