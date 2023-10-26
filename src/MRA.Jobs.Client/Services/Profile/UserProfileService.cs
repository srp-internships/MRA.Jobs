using System.Net;
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

    public async Task<string> Update(UpdateProfileCommand command)
    {
        try
        {
            var result = await _identityHttpClient.PutAsJsonAsync("Profile", command);

            if (result.IsSuccessStatusCode)
                return "";

            if (result.StatusCode == HttpStatusCode.BadRequest)
                return result.RequestMessage.ToString();

            return "Server is not responding, please try later";

        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return "Server is not responding, please try later";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return "An error occurred";
        }
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

    public async Task<HttpResponseMessage> DeleteEducationAsync(Guid id)
    {
        var respose = await _identityHttpClient.DeleteAsync($"Profile/DeleteEducationDetail/{id}");
        return respose;
    }

    public async Task<HttpResponseMessage> DeleteExperienceAsync(Guid id)
    {
        var respose = await _identityHttpClient.DeleteAsync($"Profile/DeleteExperienceDetail/{id}");
        return respose;
    }

    public async Task<List<UserExperienceResponse>> GetExperiences()
    {
        var result = await _identityHttpClient.GetJsonAsync<List<UserExperienceResponse>>("Profile/GetExperiencesByUser");
        return result;
    }

    public async Task<HttpResponseMessage> CreateExperienceAsync(CreateExperienceDetailCommand command)
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

    public async Task<HttpResponseMessage> RemoveSkillAsync(string skill)
    {
        var response = await _identityHttpClient.DeleteAsync($"Profile/RemoveUserSkill/{skill}");
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
