using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Client.Services.Profile;

public interface IUserProfile
{
    Task<UserProfileResponse> Get();
    Task<HttpResponseMessage> Update(UpdateProfileCommand command);
}
