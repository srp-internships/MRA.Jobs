using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using MRA.Identity.Application.Contract.Profile;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.IntegrationTests.FakeClasses;

public class FakeHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        var mockedHandler = new Mock<HttpMessageHandler>();

        var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        var expectedResponseList = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

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
            case "IdentityHttpClientUsers":
                {
                    var users = new List<UserResponse>();
                    for (int i = 0; i < 15; i++)
                    {
                        users.Add(new UserResponse
                        {
                            Id = Guid.NewGuid(),
                            UserName = $"User{i}",
                            FullName = $"Full Name {i}",
                            DateOfBirth = new DateTime(1997, 12, 1),
                            Email = $"user{i}@example.com",
                            PhoneNumber = $"+99292555{i.ToString().PadLeft(4, '0')}",
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                        });
                    }

                    expectedResponse.Content = JsonContent.Create(new PagedList<UserResponse>
                    {
                        TotalCount = 15, PageSize = 10, Items = users
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
            case "IdentityHttpClientUsersList":
                {
                    var users = new List<UserResponse>();
                    for (int i = 0; i < 15; i++)
                    {
                        users.Add(new UserResponse
                        {
                            Id = Guid.NewGuid(),
                            UserName = $"User{i}",
                            FullName = $"Full Name {i}",
                            DateOfBirth = new DateTime(1997, 12, 1),
                            Email = $"user{i}@example.com",
                            PhoneNumber = $"+99292555{i.ToString().PadLeft(4, '0')}",
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                        });
                    }

                    expectedResponseList.Content = JsonContent.Create(users);

                    mockedHandler
                        .Protected()
                        .Setup<Task<HttpResponseMessage>>(
                            "SendAsync",
                            ItExpr.IsAny<HttpRequestMessage>(),
                            ItExpr.IsAny<CancellationToken>())
                        .ReturnsAsync(expectedResponseList);
                    return new HttpClient(mockedHandler.Object);
                }
            default:
                return new HttpClient();
        }
    }
}