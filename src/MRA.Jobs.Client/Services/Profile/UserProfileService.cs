using System.Net;
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
using MRA.Identity.Application.Contract.User.Queries;
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

    public async Task<UserProfileResponse> Get(string userName = null)
    {
        var result =
            await _identityHttpClient.GetAsync(
                $"Profile{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}");
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<UserProfileResponse>();
        }

        return null;
    }

    public async Task<List<UserEducationResponse>> GetEducationsByUser()
    {
        var result =
            await _identityHttpClient.GetFromJsonAsync<List<UserEducationResponse>>("Profile/GetEducationsByUser");
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

    public async Task<List<UserExperienceResponse>> GetExperiencesByUser()
    {
        var result =
            await _identityHttpClient.GetFromJsonAsync<List<UserExperienceResponse>>("Profile/GetExperiencesByUser");
        return result;
    }

    public async Task<HttpResponseMessage> CreateExperienceAsync(CreateExperienceDetailCommand command)
    {
        var response = await _identityHttpClient.PostAsJsonAsync("Profile/CreateExperienceDetail", command);
        return response;
    }

    public async Task<HttpResponseMessage> UpdateExperienceAsync(UpdateExperienceDetailCommand command)
    {
        var response = await _identityHttpClient.PutAsJsonAsync("Profile/UpdateExperienceDetail", command);
        return response;
    }

    public async Task<UserSkillsResponse> GetUserSkills(string userName = null)
    {
        var response = await _identityHttpClient
            .GetFromJsonAsync<UserSkillsResponse>(
                $"Profile/GetUserSkills{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}");
        return response;
    }

    public async Task<HttpResponseMessage> RemoveSkillAsync(string skill)
    {
        var response = await _identityHttpClient.DeleteAsync($"Profile/RemoveUserSkill/{Uri.EscapeDataString(skill)}");
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

    public async Task<bool> SendConfirmationCode(string phoneNumber)
    {
        var response = await _identityHttpClient.GetFromJsonAsync<bool>($"sms/send_code?PhoneNumber={phoneNumber}");

        return response;
    }

    public async Task<SmsVerificationCodeStatus> CheckConfirmationCode(string phoneNumber, int? code)
    {
        var response =
            await _identityHttpClient.GetFromJsonAsync<SmsVerificationCodeStatus>(
                $"sms/verify_code?PhoneNumber={phoneNumber}&Code={code}");
        return response;
    }

    public async Task<List<UserEducationResponse>> GetAllEducations()
    {
        var result =
            await _identityHttpClient.GetFromJsonAsync<List<UserEducationResponse>>("Profile/GetAllEducations");
        return result;
    }

    public async Task<List<UserExperienceResponse>> GetAllExperiences()
    {
        var result =
            await _identityHttpClient.GetFromJsonAsync<List<UserExperienceResponse>>("Profile/GetAllExperiences");
        return result;
    }

    public async Task<UserSkillsResponse> GetAllSkills()
    {
        var response = await _identityHttpClient.GetFromJsonAsync<UserSkillsResponse>("Profile/GetAllSkills");
        return response;
    }
}
