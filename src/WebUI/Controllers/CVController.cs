using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.CV.Commands;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CVController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCV([FromBody] CreateCVCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }
}
