using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Applications.Candidates;

namespace MRA.Jobs.Application.IntegrationTests.FakeClasses;
public class TestUsersService : IUsersService
{
    public async Task<List<UserResponse>> GetUsersAsync(GetCandidatesQuery query)
    {
        List<UserResponse> userResponses = new List<UserResponse>
        {
            new UserResponse
            {
                Id = Guid.NewGuid(),
                UserName = "Bob",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                DateOfBirth = DateTime.Now,
                Email = "email@gmail.com",
                PhoneNumber = "+9999999999",
                FullName = "Ali Aliev Alivich"
            }
        };
        return userResponses;
    }

}
