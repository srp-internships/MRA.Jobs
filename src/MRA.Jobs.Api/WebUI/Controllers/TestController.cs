using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands.CreateTest;
using MRA.Jobs.Application.Contracts.Tests.Commands.CreateTestResult;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestController : ApiControllerBase
{
    [HttpPost("{slug}/test")]
    public async Task<ActionResult<TestInfoDto>> SendTestCreationRequest([FromRoute] string slug, [FromBody] CreateTestCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/test/result")]
    public async Task<ActionResult<TestResultDto>> GetTestResultRequest([FromRoute] string slug, [FromBody] CreateTestResultCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
