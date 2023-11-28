using System.Net;
using System.Text;
using Moq;
using Moq.Protected;

namespace MRA.Jobs.Application.IntegrationTests.FakeClasses;

public class FakeHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        var mockedHandler = new Mock<HttpMessageHandler>();

        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(new MemoryStream("hello world!!!"u8.ToArray())),
        };


        mockedHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        switch (name)
        {
            case "IdentityHttpClient":
                return new HttpClient(mockedHandler.Object);
            default:
                return new HttpClient();
        }
    }
}