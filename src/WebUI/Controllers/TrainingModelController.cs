using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrainingModelController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewTrainingModel(CreateTrainingModelCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateTrainingModel([FromRoute] Guid id, [FromBody] UpdateTrainingModelCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTrainingModel([FromRoute] Guid id, [FromBody] DeleteTrainingModelCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
