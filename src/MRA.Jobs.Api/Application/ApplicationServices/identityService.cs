using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Application.ApplicationServices;
public class IdentityService : IidentityService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _factory;

    public IdentityService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientFactory factory)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _factory = factory;
    }

    public async Task<UserProfileResponse> ApplicantDetailsInfo()
    {
        using var identityHttpClient = _factory.CreateClient();
        var applicantDetails = new UserProfileResponse();
        identityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext?.Request.Headers.Authorization[0]!.Split(' ')[1]);
        using var response = (await identityHttpClient.GetAsync(_configuration["IdentityApi:Profile"]));
        if (response.IsSuccessStatusCode)
        {
            applicantDetails = await response.Content.ReadFromJsonAsync<UserProfileResponse>();
        }
        return applicantDetails;
    }


}
