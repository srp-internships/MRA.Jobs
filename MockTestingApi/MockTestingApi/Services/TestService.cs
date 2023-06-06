using MockTestingApi.Entities;

namespace MockTestingApi.Services;

public class TestService : ITestService
{
    public CreateTestResponse CreateTest(CreateTestRequest request)
    {
        return new CreateTestResponse
        {
            TestId = Guid.NewGuid(),
            MaxScore = new Random().Next(1, 100)
        };
    }

    public async Task PassTest(PassTestRequest request)
    {
        throw new NotImplementedException();
    }
}
