using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InternshipsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewInternship(CreateInternshipCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateInternship([FromRoute] Guid id, [FromBody] UpdateInternshipCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteInternship([FromRoute] Guid id, [FromBody] DeleteInternshipCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
