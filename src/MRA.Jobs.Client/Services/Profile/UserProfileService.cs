using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
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
using MRA.Jobs.Client.Services.HttpClients;
using Newtonsoft.Json;

namespace MRA.Jobs.Client.Services.Profile;

public class UserProfileService(IdentityApiHttpClientService identityHttpClient, AuthenticationStateProvider authenticationStateProvider) : IUserProfileService
{
    public async Task<string> Update(UpdateProfileCommand command)
    {
        try
        {
            var result = await identityHttpClient.PutAsJsonAsync<bool>("Profile", command);

            if (result.Success)
                return "";

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return result.Result.ToString();

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
        await authenticationStateProvider.GetAuthenticationStateAsync();
        var result =
            await identityHttpClient.GetAsJsonAsync<UserProfileResponse>(
                $"Profile{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}");
        if (result.Success)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<ApiResponse<List<UserEducationResponse>>> GetEducationsByUser()
    {
        var result =
            await _identityHttpClient.GetFromJsonAsync<List<UserEducationResponse>>("Profile/GetEducationsByUser");
        return result;
    }

    public async Task<ApiResponse<bool>> CreateEducationAsync(CreateEducationDetailCommand command)
    {
        var response = await identityHttpClient.PostAsJsonAsync<bool>("Profile/CreateEducationDetail", command);
        return response;
    }

    public async Task<ApiResponse<Guid>> UpdateEducationAsync(UpdateEducationDetailCommand command)
    {
        var response = await identityHttpClient.PutAsJsonAsync<Guid>("Profile/UpdateEducationDetail", command);
        return response;
    }

    public async Task<ApiResponse> DeleteEducationAsync(Guid id)
    {
        var respose = await identityHttpClient.DeleteAsync($"Profile/DeleteEducationDetail/{id}");
        return respose;
    }

    public async Task<ApiResponse<List<UserExperienceResponse>>> GetExperiencesByUser()
    {
        var result =
            await identityHttpClient.GetAsJsonAsync<List<UserExperienceResponse>>("Profile/GetExperiencesByUser");
        return result;
    }

    public async Task<ApiResponse<Guid>> CreateExperienceAsync(CreateExperienceDetailCommand command)
    {
        var response = await identityHttpClient.PostAsJsonAsync<Guid>("Profile/CreateExperienceDetail", command);
        return response;
    }

    public async Task<ApiResponse<Guid>> UpdateExperienceAsync(UpdateExperienceDetailCommand command)
    {
        var response = await identityHttpClient.PutAsJsonAsync<Guid>("Profile/UpdateExperienceDetail", command);
        return response;
    }

    public async Task<UserSkillsResponse> GetUserSkills(string userName = null)
    {
        var response = await identityHttpClient
            .GetAsJsonAsync<UserSkillsResponse>(
                $"Profile/GetUserSkills{(userName != null ? "?userName=" + Uri.EscapeDataString(userName) : "")}");
        return response.Result;

    }

    public async Task<ApiResponse> RemoveSkillAsync(string skill)
    {
        var response = await identityHttpClient.DeleteAsync($"Profile/RemoveUserSkill/{Uri.EscapeDataString(skill)}");
        return response;
    }

    public async Task<UserSkillsResponse> AddSkills(AddSkillsCommand command)
    {
        var response = await identityHttpClient.PostAsJsonAsync<UserSkillsResponse>("Profile/AddSkills", command);
        return response.Success ? response.Result : null;
    }

    public async Task<ApiResponse<bool>> SendConfirmationCode(string phoneNumber)
    {
        var response = await identityHttpClient.GetAsJsonAsync<bool>($"sms/send_code?PhoneNumber={phoneNumber}");

        return response;
    }

    public async Task<ApiResponse<SmsVerificationCodeStatus>> CheckConfirmationCode(string phoneNumber, int? code)
    {
        var response =
            await identityHttpClient.GetAsJsonAsync<SmsVerificationCodeStatus>(
                $"sms/verify_code?PhoneNumber={phoneNumber}&Code={code}");
        return response;
    }

    public async Task<ApiResponse<List<UserEducationResponse>>> GetAllEducations()
    {
        var result =
            await identityHttpClient.GetAsJsonAsync<List<UserEducationResponse>>("Profile/GetAllEducations");
        return result;
    }

    public async Task<ApiResponse<List<UserExperienceResponse>>> GetAllExperiences()
    {
        var result =
            await identityHttpClient.GetAsJsonAsync<List<UserExperienceResponse>>("Profile/GetAllExperiences");
        return result;
    }

    public async Task<ApiResponse<UserSkillsResponse>> GetAllSkills()
    {
        var response = await identityHttpClient.GetAsJsonAsync<UserSkillsResponse>("Profile/GetAllSkills");
        return response;
    }
}
