using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
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
