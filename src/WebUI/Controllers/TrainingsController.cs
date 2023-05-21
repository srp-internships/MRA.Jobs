using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrainingsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingModelById(Guid id)
    {
        var trainingModel = await Mediator.Send(new TrainingModelDetailsDTO { Id = id });
        return Ok(trainingModel);
    }

    [HttpGet]
    public async Task<ActionResult<PaggedList<TrainingVacancyListDTO>>> GetTrainingModelsWithPagination([FromQuery] PaggedListQuery<TrainingVacancyListDTO> query)
    {
        var trainingModels = await Mediator.Send(query);
        return Ok(trainingModels);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewTrainingModel(CreateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateTrainingModel([FromRoute] Guid id, [FromBody] UpdateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTrainingModel([FromRoute] Guid id, [FromBody] DeleteTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
