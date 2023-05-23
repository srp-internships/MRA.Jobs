using MockTestingApi.Entities;

namespace MockTestingApi.Services;

public interface ITestService
{
    Task<CreateTestResponse> CreateTest(CreateTestRequest request);
    Task PassTest(PassTestRequest request);
}
