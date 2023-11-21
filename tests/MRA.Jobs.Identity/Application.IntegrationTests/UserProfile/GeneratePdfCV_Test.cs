using System.Net;

namespace MRA.Jobs.Application.IntegrationTests.UserProfile;
public class GeneratePdfCV_Test : BaseTest
{
    [Test]
    public async Task GeneratePdfCv_ShouldReturnFile_Success()
    {
        await AddApplicantAuthorizationAsync();
        var response = await _client.GetAsync("/api/Profile/GenerateCV");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsByteArrayAsync();
        Assert.IsTrue(content.Length > 0, "File not received");
    }

    [Test]
    public async Task GeneratePdfCv_ShouldReturnFile_Unauthorized()
    {
        var response = await _client.GetAsync("/api/Profile/GenerateCV");
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

}
