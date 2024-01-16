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
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Client.Services.Profile;

public interface IUserProfileService
{
    Task<UserProfileResponse> Get(string userName = null);
    Task<string> Update(UpdateProfileCommand command);

    Task<ApiResponse<List<UserEducationResponse>>> GetEducationsByUser();
    Task<ApiResponse<List<UserEducationResponse>>> GetAllEducations();

    Task<ApiResponse<bool>> CreateEducationAsync(CreateEducationDetailCommand command);

    Task<ApiResponse<Guid>> UpdateEducationAsync(UpdateEducationDetailCommand command);
    Task<ApiResponse> DeleteEducationAsync(Guid id);

    Task<ApiResponse<List<UserExperienceResponse>>> GetExperiencesByUser();
    Task<ApiResponse<List<UserExperienceResponse>>> GetAllExperiences();

    Task<ApiResponse<Guid>> CreateExperienceAsync(CreateExperienceDetailCommand command);

    Task<ApiResponse<Guid>> UpdateExperienceAsync(UpdateExperienceDetailCommand command);

    Task<UserSkillsResponse> GetUserSkills(string userName = null);
    Task<ApiResponse<UserSkillsResponse>> GetAllSkills();
    Task<ApiResponse> RemoveSkillAsync(string skill);

    Task<UserSkillsResponse> AddSkills(AddSkillsCommand command);

    Task<ApiResponse<bool>> SendConfirmationCode(string phoneNumber);
    Task<ApiResponse<SmsVerificationCodeStatus>> CheckConfirmationCode(string phoneNumber, int? code);
}
