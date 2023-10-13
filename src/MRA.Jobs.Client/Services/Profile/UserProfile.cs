using System.Net.Http.Json;
using MatBlazor;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Client.Services.Profile;

public class UserProfile : IUserProfile
{
    private readonly IdentityHttpClient _identityHttpClient;

    public UserProfile(IdentityHttpClient identityHttpClient)
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
}
