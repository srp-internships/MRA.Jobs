using MatBlazor;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Responses;
using Newtonsoft.Json;

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

    public async Task<HttpResponseMessage> DeleteEducationAync(Guid id)
    {
        var respose = await _identityHttpClient.DeleteAsync($"Profile/DeleteEducationDetail/{id}");
        return respose;
    }

    public async Task<HttpResponseMessage> DeleteExperienceAync(Guid id)
    {
        var respose = await _identityHttpClient.DeleteAsync($"Profile/DeleteExperienceDetail/{id}");
        return respose;
    }

    public async Task<List<UserExperienceResponse>> GetExperiences()
    {
        var result = await _identityHttpClient.GetJsonAsync<List<UserExperienceResponse>>("Profile/GetExperiencesByUser");
        return result;
    }

    public async Task<HttpResponseMessage> CreateExperienceAsycn(CreateExperienceDetailCommand command)
    {
        var response = await _identityHttpClient.PostAsJsonAsync("Profile/СreateExperienceDetail", command);
        return response;
    }

    public async Task<HttpResponseMessage> UpdateExperienceAsync(UpdateExperienceDetailCommand command)
    {
        var response = await _identityHttpClient.PutAsJsonAsync("Profile/UpdateExperienceDetail", command);
        return response;
    }

    public async Task<UserSkillsResponse> GetSkills()
    {
        var response = await _identityHttpClient.GetJsonAsync<UserSkillsResponse>("Profile/GetUserSkills");
        return response;
    }

    public async Task<HttpResponseMessage> RemoveSkillAync(string skill)
    {
       var response =await _identityHttpClient.DeleteAsync($"Profile/RemoveUserSkill/{skill}");
        return response;
    }

    public async Task<UserSkillsResponse> AddSkills(AddSkillsCommand command)
    {
        var response = await _identityHttpClient.PostAsJsonAsync("Profile/AddSkills", command);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserSkillsResponse>(content);
            return result;
        }
        return null;
    }
}
