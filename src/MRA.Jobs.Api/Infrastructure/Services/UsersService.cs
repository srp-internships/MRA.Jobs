using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Infrastructure.Services;

public class UsersService(
    ICurrentUserService currentUserService,
    IHttpClientFactory clientFactory,
    IConfiguration configuration) : IUsersService
{
    public async Task<List<UserResponse>> GetUsersAsync(string fullName = null, string email = null,
        string phoneNumber = null, string skills = null)
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
        if (!fullName.IsNullOrEmpty()) queryParameters.Add("FullName", fullName.Trim());
        if (!phoneNumber.IsNullOrEmpty()) queryParameters.Add("PhoneNumber", phoneNumber.Trim());
        if (!email.IsNullOrEmpty()) queryParameters.Add("Email", email.Trim());
        if (!skills.IsNullOrEmpty()) queryParameters.Add("Skills", skills.Trim());

        var queryString = QueryHelpers.AddQueryString(configuration["MraJobs-IdentityApi:Users"], queryParameters);

        users = await httpClient.GetFromJsonAsync<List<UserResponse>>(queryString);


        return users;
    }
}