using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;

namespace MRA.Jobs.Client.Services.Profile;

public interface IUserProfileService
{
    Task<UserProfileResponse> Get(string userName=null);
   
    Task<ApiResponse<List<UserEducationResponse>>> GetEducationsByUser(string userName);
  
    Task<ApiResponse<List<UserExperienceResponse>>> GetExperiencesByUser(string userName);
    
    Task<UserSkillsResponse> GetUserSkills(string userName = null);
}
