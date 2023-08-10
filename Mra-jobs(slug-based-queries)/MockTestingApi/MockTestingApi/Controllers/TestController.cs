using Microsoft.AspNetCore.Mvc;
using MockTestingApi.Entities;
using MockTestingApi.Services;

namespace MockTestingApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpPost("create")]
    public ActionResult<CreateTestResponse> CreateTest([FromBody] CreateTestRequest request)
    {
        return _testService.CreateTest(request);
    }

    [HttpPost("pass")]
    public async Task<ActionResult<PassTestRequest>> PassTest([FromBody] PassTestRequest request)
    {
        var result = await Task.FromResult(request);
        return Ok(result);
    }
}
