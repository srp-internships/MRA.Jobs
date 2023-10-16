using System.Net.Http.Json;
using MatBlazor;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Client.Services.Profile;

public class UserProfileService : IUserProfileService
{
    private readonly IdentityHttpClient _identityHttpClient;

    public UserProfileService(IdentityHttpClient identityHttpClient)
    {
        _identityHttpClient = identityHttpClient;
    }

    public async Task<HttpResponseMessage> Update(UpdateProfileCommand command)
    {
        return await _identityHttpClient.PutAsJsonAsync("Profile", command);
    }

    public async Task<UserProfileResponse> Get()
    {
        var result = await _identityHttpClient.GetJsonAsync<UserProfileResponse>("Profile");
        return result;
    }

    public async Task<List<UserEducationResponse>> GetEducations()
    {
        var result = await _identityHttpClient.GetJsonAsync<List<UserEducationResponse>>("Profile/GetEducationsByUser");
        return result;
    }

    public async Task<HttpResponseMessage> CreateEducationAsуnc(CreateEducationDetailCommand command)
    {
        var response = await _identityHttpClient.PostAsJsonAsync("Profile/CreateEducationDetail", command);
        return response;
    }

    public async Task<HttpResponseMessage> UpdateEducationAsync(UpdateEducationDetailCommand command)
    {
        var response = await _identityHttpClient.PutAsJsonAsync("Profile/UpdateEducationDetail", command);
        return response;
    }
}
