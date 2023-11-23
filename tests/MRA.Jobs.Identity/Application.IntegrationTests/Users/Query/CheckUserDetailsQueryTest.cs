using System.Net.Http.Json;
using MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Application.IntegrationTests.Users.Query;
public class CheckUserDetailsQueryTest : BaseTest
{
    [TestCase("test@example.com", "+992111111111", "@Alex123", false)]
    [TestCase("test2123@example.com", "+992522222222", "@Bob456", true)]
    [TestCase("test31@example.com", "+992333333333", "@Charlie789", true)]
    [TestCase("test1@example.com", "+992123456789", "@Alex33", false)]
    [TestCase("reviewer@example.com", "+992223456789", "@Reviewer", false)]
    public async Task CheckUserDetailsQuery_UserDataAvailability(string email, string phoneNumber, string userName, bool expectedResult)
    {
        var query = new CheckUserDetailsQuery()
        {
            Email = email,
            PhoneNumber = phoneNumber,
            UserName = userName,
        };
        var response = await _client.GetAsync($"/api/User/CheckUserDetails/{query.UserName}/{query.PhoneNumber}/{query.Email}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UserDetailsResponse>();

        bool isDataAvailable = !result.IsEmailTaken && !result.IsPhoneNumberTaken && !result.IsUserNameTaken;
        Assert.That(isDataAvailable, Is.EqualTo(expectedResult), "Data availability result is not as expected. " +
            $" IsUserNameTaken: {result.IsUserNameTaken}" +
            $" IsPhoneNumberTaken: {result.IsPhoneNumberTaken}" +
            $" IsEmailTaken: {result.IsEmailTaken}");
    }

}
