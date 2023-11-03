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

namespace MRA.Jobs.Client.Services.Profile;

public interface IUserProfileService
{
    Task<UserProfileResponse> Get();
    Task<string> Update(UpdateProfileCommand command);

    Task<List<UserEducationResponse>> GetEducationsByUser();
    Task<List<UserEducationResponse>> GetAllEducations();

    Task<HttpResponseMessage> CreateEducationAsуnc(CreateEducationDetailCommand command);

    Task<HttpResponseMessage> UpdateEducationAsync(UpdateEducationDetailCommand command);
    Task<HttpResponseMessage> DeleteEducationAsync(Guid id);
    Task<HttpResponseMessage> DeleteExperienceAsync(Guid id);

    Task<List<UserExperienceResponse>> GetExperiencesByUser();
    Task<List<UserExperienceResponse>> GetAllExperiences();

    Task<HttpResponseMessage> CreateExperienceAsync(CreateExperienceDetailCommand command);

    Task<HttpResponseMessage> UpdateExperienceAsync(UpdateExperienceDetailCommand command);

    Task<UserSkillsResponse> GetUserSkills();
    Task<UserSkillsResponse> GetAllSkills();
    Task<HttpResponseMessage> RemoveSkillAsync(string skill);

    Task<UserSkillsResponse> AddSkills(AddSkillsCommand command);

    Task<bool> SendConfirmationCode(string phoneNumber);
    Task<SmsVerificationCodeStatus> CheckConfirmationCode(string phoneNumber, int? code);
}
