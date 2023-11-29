using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Application.ApplicationServices;
public class IdentityService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
        IHttpClientFactory factory)
    : IidentityService
{
    public async Task<UserProfileResponse> ApplicantDetailsInfo()
    {
        using var identityHttpClient = factory.CreateClient("IdentityHttpClientProfile");
        var applicantDetails = new UserProfileResponse();
        identityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext?.Request.Headers.Authorization[0]!.Split(' ')[1]);
        using var response = await identityHttpClient.GetAsync(configuration["IdentityApi:Profile"]);
        if (response.IsSuccessStatusCode)
        {
            applicantDetails = await response.Content.ReadFromJsonAsync<UserProfileResponse>();
        }
        return applicantDetails;
    }


}
