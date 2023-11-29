using System.Net;
using System.Net.Http.Json;
using System.Text;
using Moq;
using Moq.Protected;
using MRA.Identity.Application.Contract.Profile;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Application.IntegrationTests.FakeClasses;

public class FakeHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        var mockedHandler = new Mock<HttpMessageHandler>();

        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };


        switch (name)
        {
            case "IdentityHttpClient":
            {
                expectedResponse.Content = new StreamContent(new MemoryStream("hello world!!!"u8.ToArray()));
                mockedHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(expectedResponse);
                return new HttpClient(mockedHandler.Object);
            }

            case "IdentityHttpClientProfile":
            {
                expectedResponse.Content = JsonContent.Create(new UserProfileResponse
                {
                    UserName = "newUsername",
                    Gender = Gender.Male,
                    DateOfBirth = DateTime.Today,
                    FirstName = "null",
                    LastName = "null",
                    AboutMyself = "null",
                    PhoneNumber = "+992925559999",
                    Email = "afhsdfk@jfsdalkf",
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false
                });
                mockedHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(expectedResponse);
                return new HttpClient(mockedHandler.Object);
            }
            default:
                return new HttpClient();
        }
    }
}