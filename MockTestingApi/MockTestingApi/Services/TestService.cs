using MockTestingApi.Entities;

namespace MockTestingApi.Services;

public class TestService : ITestService
{
    public Task<CreateTestResponse> CreateTest(CreateTestRequest request)
    {
        throw new NotImplementedException();
    }

    public Task PassTest(PassTestRequest request)
    {
        throw new NotImplementedException();
    }
}
