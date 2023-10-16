using System.Runtime.CompilerServices;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Client.Services.Profile;

public interface IUserProfileService
{
    Task<UserProfileResponse> Get();
    Task<HttpResponseMessage> Update(UpdateProfileCommand command);

    Task<List<UserEducationResponse>> GetEducations();

    Task<HttpResponseMessage> CreateEducationAsуnc(CreateEducationDetailCommand command);

    Task<HttpResponseMessage> UpdateEducationAsync(UpdateEducationDetailCommand command);
    Task<HttpResponseMessage> DeleteEducationAync(Guid id); 
    Task<HttpResponseMessage> DeleteExperienceAync(Guid id);

    Task<List<UserExperienceResponse>> GetExperiences();

    Task<HttpResponseMessage> CreateExperienceAsycn(CreateExperienceDetailCommand command);
}
