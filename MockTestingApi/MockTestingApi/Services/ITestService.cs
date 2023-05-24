using MockTestingApi.Entities;

namespace MockTestingApi.Services;

public interface ITestService
{
    CreateTestResponse CreateTest(CreateTestRequest request);
    Task PassTest(PassTestRequest request);
}
