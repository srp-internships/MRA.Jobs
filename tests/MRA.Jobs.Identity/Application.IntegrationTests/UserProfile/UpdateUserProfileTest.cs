using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Profile;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;

namespace MRA.Jobs.Application.IntegrationTests.UserProfile;
public class UpdateUserProfileTest : BaseTest
{
    [Test]
    public async Task UpdateUserProfile_ShouldUpdateUserProfile_Success()
    {
        await AddApplicantAuthorizationAsync();
        var profile = new UpdateProfileCommand()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Email = Applicant.Email,
            Gender = Gender.Male,
            DateOfBirth = DateTime.Now,
            PhoneNumber = "+992123456789"
        };
        var response = await _client.PutAsJsonAsync("/api/Profile", profile);

        response.EnsureSuccessStatusCode();
    }
}
