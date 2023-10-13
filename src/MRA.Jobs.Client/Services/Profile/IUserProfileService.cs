using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Client.Services.Profile;

public interface IUserProfileService
{
    Task<UserProfileResponse> Get();
    Task<HttpResponseMessage> Update(UpdateProfileCommand command);

    Task<List<UserEducationResponse>> GetEducations();
}
