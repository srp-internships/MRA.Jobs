using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrainingModelController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingModelById(Guid id)
    {
        var trainingModel = await Mediator.Send(new TrainingModelDetailsDTO { Id = id });
        return Ok(trainingModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetTrainingModelsWithPagination([FromQuery] PaggedListQuery<TrainingModelListDTO> query)
    {
        var trainingModels = await Mediator.Send(query);
        return Ok(trainingModels);
    }

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
